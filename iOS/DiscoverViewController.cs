using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Foundation;
using HypeFramework;
using UIKit;
using Newtonsoft.Json;

namespace Lounging
{
	public partial class DiscoverViewController : UIViewController//, IUITableViewDelegate, IUITableViewDataSource
	{
		HYP hype;
		NetworkObserver networkObserver;
		StateObserver stateObserver;
		MessageObserver messageObserver;
		public static NSDictionary HypeOptions = new NSDictionary("HYPOptionRealmKey", "baea7364");
		public static Dictionary<string, HYPInstance> FoundInstances;
		Dictionary<string, Lounge> FoundLounges;
		//UITableView TableView;
		protected DiscoverViewController(IntPtr handle) : base(handle)
		{
			// Note: this .ctor should not contain any initialization logic.	

		}

		public DiscoverViewController()
		{
			
		}

		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			hype = HYP.Instance();
			networkObserver = new NetworkObserver(this);
			stateObserver = new StateObserver(this);
			messageObserver = new MessageObserver(this);
			FoundInstances = new Dictionary<string, HYPInstance>();
			FoundLounges = new Dictionary<string, Lounge>();
			RequestHypeToStart();
		}

		public override void ViewDidAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			RequestHypeToStart();
		}

		public override void ViewWillAppear(bool animated)
		{
			base.ViewWillAppear(animated);
			loungesView.Source = new LoungesTableViewSource(FoundLounges, loungesView, hype.DomesticInstance);
		}

		public override void DidReceiveMemoryWarning()
		{
			base.DidReceiveMemoryWarning();
			// Release any cached data, images, etc that aren't in use.
		}

		#region Private Actions

		private void RequestHypeToStart()
		{
			hype.AddNetworkObserver(networkObserver);
			hype.AddStateObserver(stateObserver);
			hype.AddMessageObserver(messageObserver);
			hype.StartWithOptions(HypeOptions);
		}

		private static void HandleNSError(NSError error, UIAlertView alertView)
		{
			StringBuilder errorMessage = new StringBuilder();

			errorMessage.AppendLine(error.LocalizedDescription);
			errorMessage.AppendLine(error.LocalizedFailureReason);
			errorMessage.AppendLine(error.LocalizedRecoverySuggestion);

			Console.WriteLine(errorMessage);
			alertView.Message = errorMessage.ToString();
			alertView.Show();
		}

		public void SendToInstance<T>(T objectToSend, HYPInstance instance, LoungeMessageType messageType) where T : class
		{
			if (instance == null) return;

			bool isList = IsInstanceOfGenericType(typeof(List<>), objectToSend);
			
			string serializedObject = JsonConvert.SerializeObject(objectToSend);
			var message = new LoungeMessage(serializedObject, (int)messageType, isList);
			string serializedMessage = JsonConvert.SerializeObject(message);

			NSData data = NSData.FromString(serializedMessage, NSStringEncoding.UTF8);
			hype.SendData(data, instance);
		}

		public void SendToInstances<T>(T objectToSend, List<HYPInstance> instances, LoungeMessageType messageType) where T : class
		{
			if (instances.Count == 0) return;

			bool isList = IsInstanceOfGenericType(typeof(List<>), objectToSend);

			string serializedObject = JsonConvert.SerializeObject(objectToSend);
			var message = new LoungeMessage(serializedObject, (int)messageType, isList);
			string serializedMessage = JsonConvert.SerializeObject(message);

			NSData data = NSData.FromString(serializedMessage, NSStringEncoding.UTF8);
			foreach (var instance in instances)
			{
				hype.SendData(data, instance);
			}
		}

		static bool IsInstanceOfGenericType(Type genericType, object instance)
		{
			Type type = instance.GetType();
			while (type != null)
			{
				if (type.IsGenericType &&
					type.GetGenericTypeDefinition() == genericType)
				{
					return true;
				}
				type = type.BaseType;
			}
			return false;
		}

		public void DecodeReceivedMessage(HYPMessage message)
		{
			var loungeMessageString = new NSString(message.Data, NSStringEncoding.UTF8).ToString();
			LoungeMessage loungeMessage = JsonConvert.DeserializeObject<LoungeMessage>(loungeMessageString);

			switch (loungeMessage.LoungeMessageType)
			{
				case (int)LoungeMessageType.LoungeDiscovery:
					if (loungeMessage.IsList)
						ReceiveLoungeDiscoveryMessage(
							JsonConvert.DeserializeObject<List<Lounge>>(loungeMessage.JsonPayload));
					else
						ReceiveLoungeDiscoveryMessage(
							JsonConvert.DeserializeObject<Lounge>(loungeMessage.JsonPayload));
				break;
			}

		}

		void ReceiveLoungeDiscoveryMessage(List<Lounge> lounges)
		{
			foreach (var lounge in lounges)
			{
				if (!FoundLounges.ContainsKey(lounge.Id))
					FoundLounges.Add(lounge.Id, lounge);
			}
			loungesView.ReloadData();
		}

		void ReceiveLoungeDiscoveryMessage(Lounge lounge)
		{
			if (lounge != null && !FoundLounges.ContainsKey(lounge.Id))
				FoundLounges.Add(lounge.Id, lounge);
			loungesView.ReloadData();
		}
		#endregion

		#region Public Actions

		public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
		{
			if (segue.Identifier == "NewLoungeSegue")
			{ // set in Storyboard
				var navctlr = segue.DestinationViewController as LoungeDetailViewController;
				if (navctlr != null)
				{
					//var source = TableView.Source as RootTableSource;
					//var rowPath = TableView.IndexPathForSelectedRow;
					//var item = source.GetItem(rowPath.Row);
					navctlr.SetTask(this, new Lounge(hype.DomesticInstance));
				}
			}
		}

		public void CreateNewLounge(Lounge lounge)
		{
			//Add the instance to the table view source.
			//var loungesViewSource = ((LoungesTableViewSource)loungesView.Source);
			//loungesViewSource?.AddItem(lounge);
			if (FoundLounges.ContainsKey(lounge.Id) && FoundLounges[lounge.Id].Active)
			{
				var alertView = new UIAlertView("Alert", "Cannot create lounge. Another active lounge already created", null, "OK");
				this.Add(alertView);
				alertView.Show();
			}
			else {
				FoundLounges.Add(lounge.Id, lounge);
				loungesView.ReloadData();
				NavigationController.PopViewController(true);

				SendToInstances(lounge, FoundInstances.Values.ToList(), LoungeMessageType.LoungeDiscovery);
			}
		}

		public void Cancel()
		{
			NavigationController.PopViewController(true);
		}

		#endregion
	
		#region Hype Observers
		public class StateObserver : HYPStateObserver
		{
			DiscoverViewController view;
			UIAlertView alertView;
			public StateObserver(DiscoverViewController viewHandle)
			{
				this.view = viewHandle;
				alertView = new UIAlertView("Alert", "", null, "OK");
				view.Add(alertView);
			}

			public override void HypeDidBecomeReady(HYP hype)
			{
				Console.WriteLine("HYPE DID BECOME READY!!!");
				Console.WriteLine("Restarting Hype");
				hype.StartWithOptions(HypeOptions);
			}

			public override void HypeDidFailStarting(HYP hype, NSError error)
			{
				HandleNSError(error, alertView);
			}

			public override void HypeDidStart(HYP hype)
			{
				Console.WriteLine("HYPE STARTED !!!!!!!!!!!!!!!");
				//alertView.Message = "Hype Started!";
				//alertView.Show();
			}

			public override void HypeDidStop(HYP hype, NSError error)
			{
				HandleNSError(error, alertView);
			}
		}

		public class NetworkObserver : HYPNetworkObserver
		{
			DiscoverViewController view;
			UIAlertView alertView;
			public NetworkObserver(DiscoverViewController viewHandle)
			{
				this.view = viewHandle;
				alertView = new UIAlertView("Alert", "", null, "OK");
				view.Add(alertView);
			}

			public override void DidFindInstance(HYP hype, HYPInstance instance)
			{
				FoundInstances.Add(instance.StringIdentifier, instance);
				if (view.FoundLounges.Count > 0)
					view.SendToInstance(view.FoundLounges.Values.ToList(), instance, LoungeMessageType.LoungeDiscovery);
				//Add the instance to the table view source.
				//((InstancesViewSource)view.loungesView.Source)?.AddItem(instance);

				//var newData = NSData.FromString("You have a new message from : " + hype.DomesticInstance.StringIdentifier, NSStringEncoding.UTF8);
				//HYPMessage newMessage = hype.SendData(newData, instance);
				//Console.WriteLine(newMessage);
				Console.WriteLine("Found Instance : " + instance.StringIdentifier);
				alertView.Message = "Found Instance : " + instance.StringIdentifier;
				alertView.Show();
			}

			public override void DidLoseInstance(HYP hype, HYPInstance instance, NSError error)
			{
				FoundInstances.Remove(instance.StringIdentifier);
				//Add the instance to the table view source.
				//((InstancesViewSource)view.loungesView.Source)?.RemoveItem(instance.StringIdentifier);
				Console.WriteLine(string.Format("Lost Instance : {0}. {1}", instance.StringIdentifier, error.DebugDescription));
				//HandleNSError(error, alertView);
			}
		}

		public class MessageObserver : HYPMessageObserver
		{
			DiscoverViewController view;
			UIAlertView alertView;
			public MessageObserver(DiscoverViewController viewHandle)
			{
				this.view = viewHandle;
				alertView = new UIAlertView("Alert", "", null, "OK");
				view.Add(alertView);
			}
			public override void DidFailSending(HYP hype, HYPMessage message, HYPInstance instance, NSError error)
			{
				alertView.Message = "Falied to Send Message - " + error.DebugDescription;
			}

			public override void DidReceiveMessage(HYP hype, HYPMessage message, HYPInstance instance)
			{
				view.DecodeReceivedMessage(message);
				alertView.Message = "message Received";
				alertView.Show();
			}
		}
		#endregion

		#region TableView Classes and Overrides
		public class LoungesTableViewSource : UITableViewSource
		{

			// there is NO database or storage of Tasks in this example, just an in-memory List<>
			List<Lounge> Lounges 
			{ 
				get { return LoungeDictionary.Values.ToList(); } 
			}
			Dictionary<string, Lounge> LoungeDictionary;
			string cellIdentifier = "loungeViewCell"; // set in the Storyboard
			UITableView TableView;
			HYPInstance ViewHypeInstance;

			public LoungesTableViewSource(Dictionary<string, Lounge> lounges, UITableView tableView, HYPInstance hypeInstance)
			{
				LoungeDictionary = lounges;
				TableView = tableView;
				ViewHypeInstance = hypeInstance;
			}

			public override nint RowsInSection(UITableView tableview, nint section)
			{
				return Lounges.Count;
			}
			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				// in a Storyboard, Dequeue will ALWAYS return a cell, 
				var cell = tableView.DequeueReusableCell(cellIdentifier);
				// if there are no cells to reuse, create a new one

				if (cell == null)
					cell = new UITableViewCell(UITableViewCellStyle.Subtitle, cellIdentifier);
				// now set the properties as normal
				var lounge = Lounges[indexPath.Row];
				cell.TextLabel.Text = lounge.Name;
				cell.DetailTextLabel.Text = lounge.Description;
				if (lounge.isLoungeMember(ViewHypeInstance.StringIdentifier))
					cell.Accessory = UITableViewCellAccessory.Checkmark;
				else
					cell.Accessory = UITableViewCellAccessory.None;
				return cell;
			}

			public Lounge GetItem(string id)
			{
				return Lounges.SingleOrDefault(a => a.Id == id);
			}

			public void AddItem(Lounge lounge)
			{
				Lounges.Add(lounge);
				TableView.ReloadData();
			}

			public void RemoveItem(Lounge lounge)
			{
				Lounges.Remove(lounge);
				TableView.ReloadData();
			}

			public void RemoveItem(string Id)
			{
				Lounges.RemoveAll(a => a.Id == Id);
				TableView.ReloadData();
			}
		}


		public class InstancesViewSource : UITableViewSource
		{

			// there is NO database or storage of Tasks in this example, just an in-memory List<>
			List<HYPInstance> Instances;
			string cellIdentifier = "instanceCell"; // set in the Storyboard
			UITableView TableView;
			public InstancesViewSource(List<HYPInstance> instances, UITableView tableView)
			{
				Instances = instances;
				TableView = tableView;
			}

			public override nint RowsInSection(UITableView tableview, nint section)
			{
				return Instances.Count;
			}
			public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
			{
				// in a Storyboard, Dequeue will ALWAYS return a cell, 
				var cell = tableView.DequeueReusableCell(cellIdentifier);
				// if there are no cells to reuse, create a new one

				if (cell == null)
					cell = new UITableViewCell(UITableViewCellStyle.Default, cellIdentifier);
				// now set the properties as normal
				cell.TextLabel.Text = Instances[indexPath.Row].StringIdentifier;
				return cell;
			}

			public HYPInstance GetItem(int id)
			{
				return Instances[id];
			}

			public void AddItem(HYPInstance instance)
			{
				Instances.Add(instance);
				TableView.ReloadData();
			}

			public void RemoveItem(HYPInstance instance)
			{
				Instances.Remove(instance);
				TableView.ReloadData();
			}

			public void RemoveItem(string instanceIdentifier)
			{
				Instances.RemoveAll(a => a.StringIdentifier == instanceIdentifier);
				TableView.ReloadData();
			}
		}

		#endregion

	}

}

