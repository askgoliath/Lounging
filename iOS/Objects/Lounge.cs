using System;
using System.Collections.Generic;
using HypeFramework;
using System.Linq;
using Newtonsoft.Json;

namespace Lounging
{
	public class Lounge
	{
		public Lounge()
		{
			Loungers = new List<Lounger>();
			Active = true;
			CurrentSong = new Song();
		}

		public Lounge(HYPInstance masterInstance)
		{
			Id = masterInstance.StringIdentifier;
			MasterInstance = masterInstance;
			Loungers = new List<Lounger>();
			Loungers.Add(new Lounger(masterInstance));
			Active = true;
		}

		public Lounge(HYPInstance masterInstance, string name )
		{
			Id = masterInstance.StringIdentifier;
			name = Name;
			MasterInstance = masterInstance;
			Loungers = new List<Lounger>();
			Loungers.Add(new Lounger(masterInstance));
			Active = true;
		}

		//use Realm? or generate custom Id?
		public string Id { get; set; }

		public string Name { get; set; }

		public string Description { get; set; }

		public bool Active { get; set; }

		public Song CurrentSong { get; set; } 

		[JsonIgnore]
		public HYPInstance MasterInstance { get; set; }

		public ICollection<Lounger> Loungers { get; set; }

		#region Methods
		public bool isLoungeMember(string loungerId)
		{
			return Loungers.Any(a => a.Id == loungerId);
		}
		#endregion
	}

	public class LoungeComparer : IEqualityComparer<Lounge>
	{

		public bool Equals(Lounge x, Lounge y)
		{
			//Check whether the objects are the same object. 
			if (Object.ReferenceEquals(x, y)) return true;

			//Check whether the lounge's properties are equal. 
			return x != null && y != null && x.Id.Equals(y.Id);
		}

		public int GetHashCode(Lounge obj)
		{
			//Get hash code for the Name field if it is not null. 
			int hashLoungeName = obj.Name == null ? 0 : obj.Name.GetHashCode();

			//Get hash code for the Code field. 
			int hashLoungeId = obj.Id.GetHashCode();

			//Calculate the hash code for the product. 
			return hashLoungeId ^ hashLoungeName;
		}
	}
}
