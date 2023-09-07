using CinemaSuite.Common;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace CinemaDirector
{
	public static class DirectorRuntimeHelper
	{
		public static List<Type> GetAllowedTrackTypes(TrackGroup trackGroup)
		{
			TimelineTrackGenre[] array = new TimelineTrackGenre[0];
			TrackGroupAttribute[] customAttributes = ReflectionHelper.GetCustomAttributes<TrackGroupAttribute>(trackGroup.GetType(), inherited: true);
			for (int i = 0; i < customAttributes.Length; i++)
			{
				if (customAttributes[i] != null)
				{
					array = customAttributes[i].AllowedTrackGenres;
					break;
				}
			}
			Type[] allSubTypes = GetAllSubTypes(typeof(TimelineTrack));
			List<Type> list = new List<Type>();
			for (int j = 0; j < allSubTypes.Length; j++)
			{
				TimelineTrackAttribute[] customAttributes2 = ReflectionHelper.GetCustomAttributes<TimelineTrackAttribute>(allSubTypes[j], inherited: true);
				for (int k = 0; k < customAttributes2.Length; k++)
				{
					if (customAttributes2[k] == null)
					{
						continue;
					}
					for (int l = 0; l < customAttributes2[k].TrackGenres.Length; l++)
					{
						TimelineTrackGenre timelineTrackGenre = customAttributes2[k].TrackGenres[l];
						for (int m = 0; m < array.Length; m++)
						{
							if (timelineTrackGenre == array[m])
							{
								list.Add(allSubTypes[j]);
								break;
							}
						}
					}
					break;
				}
			}
			return list;
		}

		public static List<Type> GetAllowedItemTypes(TimelineTrack timelineTrack)
		{
			CutsceneItemGenre[] array = new CutsceneItemGenre[0];
			TimelineTrackAttribute[] customAttributes = ReflectionHelper.GetCustomAttributes<TimelineTrackAttribute>(timelineTrack.GetType(), inherited: true);
			for (int i = 0; i < customAttributes.Length; i++)
			{
				if (customAttributes[i] != null)
				{
					array = customAttributes[i].AllowedItemGenres;
					break;
				}
			}
			Type[] allSubTypes = GetAllSubTypes(typeof(TimelineItem));
			List<Type> list = new List<Type>();
			for (int j = 0; j < allSubTypes.Length; j++)
			{
				CutsceneItemAttribute[] customAttributes2 = ReflectionHelper.GetCustomAttributes<CutsceneItemAttribute>(allSubTypes[j], inherited: true);
				for (int k = 0; k < customAttributes2.Length; k++)
				{
					if (customAttributes2[k] == null)
					{
						continue;
					}
					for (int l = 0; l < customAttributes2[k].Genres.Length; l++)
					{
						CutsceneItemGenre cutsceneItemGenre = customAttributes2[k].Genres[l];
						for (int m = 0; m < array.Length; m++)
						{
							if (cutsceneItemGenre == array[m])
							{
								list.Add(allSubTypes[j]);
								break;
							}
						}
					}
					break;
				}
			}
			return list;
		}

		private static Type[] GetAllSubTypes(Type ParentType)
		{
			List<Type> list = new List<Type>();
			Assembly[] assemblies = ReflectionHelper.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				Type[] types = ReflectionHelper.GetTypes(assemblies[i]);
				for (int j = 0; j < types.Length; j++)
				{
					if (types[j] != null && ReflectionHelper.IsSubclassOf(types[j], ParentType))
					{
						list.Add(types[j]);
					}
				}
			}
			return list.ToArray();
		}

		public static List<Transform> GetAllTransformsInHierarchy(Transform parent)
		{
			List<Transform> list = new List<Transform>();
			for (int i = 0; i < parent.childCount; i++)
			{
				Transform child = parent.GetChild(i);
				list.AddRange(GetAllTransformsInHierarchy(child));
				list.Add(child);
			}
			return list;
		}
	}
}
