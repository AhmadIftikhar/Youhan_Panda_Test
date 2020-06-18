//-----------------------------------------------------------------------
// <copyright file="MapMatchingParameters.cs" company="Mapbox">
//     Copyright (c) 2017 Mapbox. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using System;
using System.ComponentModel;
using Mapbox.VectorTile.Geometry;

namespace Mapbox.MapMatching
{
	/// <summary>Directions profile id</summary>
	public enum Profile
	{
		[System.ComponentModel.Description("mapbox/driving")]
		MapboxDriving,
		[System.ComponentModel.Description("mapbox/driving-traffic")]
		MapboxDrivingTraffic,
		[System.ComponentModel.Description("mapbox/walking")]
		MapboxWalking,
		[System.ComponentModel.Description("mapbox/cycling")]
		MapboxCycling
	}


	/// <summary>Format of the returned geometry. Default value 'Polyline' with precision 5.</summary>
	public enum Geometries
	{
		/// <summary>Default, precision 5.</summary>
		[System.ComponentModel.Description("polyline")]
		Polyline,
		/// <summary>Precision 6.</summary>
		[System.ComponentModel.Description("polyline6")]
		Polyline6,
		/// <summary>Geojson.</summary>
		[System.ComponentModel.Description("geojson")]
		GeoJson
	}


	/// <summary>Type of returned overview geometry. </summary>
	public enum Overview
	{
		/// <summary>The most detailed geometry available </summary>
		[System.ComponentModel.Description("full")]
		Full,
		/// <summary>A simplified version of the full geometry</summary>
		[System.ComponentModel.Description("simplified")]
		Simplified,
		/// <summary>No overview geometry </summary>
		[System.ComponentModel.Description("false")]
		None
	}


	/// <summary>Whether or not to return additional metadata along the route. Several annotations can be used.</summary>
	[System.Flags]
	public enum Annotations
	{
		[System.ComponentModel.Description("duration")]
		Duration,
		[System.ComponentModel.Description("distance")]
		Distance,
		[System.ComponentModel.Description("speed")]
		Speed,
		[System.ComponentModel.Description("congestion")]
		Congestion
	}


	/// <summary>
	/// https://www.mapbox.com/api-documentation/#retrieve-directions
	/// </summary>
	public enum InstructionLanguages
	{
		[System.ComponentModel.Description("de")]
		German,
		[System.ComponentModel.Description("en")]
		English,
		[System.ComponentModel.Description("eo")]
		Esperanto,
		[System.ComponentModel.Description("es")]
		Spanish,
		[System.ComponentModel.Description("es-ES")]
		SpanishSpain,
		[System.ComponentModel.Description("fr")]
		French,
		[System.ComponentModel.Description("id")]
		Indonesian,
		[System.ComponentModel.Description("it")]
		Italian,
		[System.ComponentModel.Description("nl")]
		Dutch,
		[System.ComponentModel.Description("pl")]
		Polish,
		[System.ComponentModel.Description("pt-BR")]
		PortugueseBrazil,
		[System.ComponentModel.Description("ro")]
		Romanian,
		[System.ComponentModel.Description("ru")]
		Russian,
		[System.ComponentModel.Description("sv")]
		Swedish,
		[System.ComponentModel.Description("tr")]
		Turkish,
		[System.ComponentModel.Description("uk")]
		Ukrainian,
		[System.ComponentModel.Description("vi")]
		Vietnamese,
		[System.ComponentModel.Description("zh-Hans")]
		ChineseSimplified
	}


}