public class V1_StellarUnits
{
	// Constants
	public const float G = 0.000000000066743015f;   // m^3 / (kg * s^2)

	// Conversion Rates
	// Length
	public const float m_km = 1/1000;
	public const float earthRadius_km = 6378.14f;	// at equator
	public const float solarRadius_km = 695700f;
	public const float AU_km = 149597870;

	// Mass
	public const float g_kg = 1/1000;
	public const float earthMass_kg = 5972200000000000000000000f;
	public const float solarMass_kg = 1988470000000000000000000000000f;

	// Time
	public const float s_Year = 1/31557600;	// using 365.25 day years
	public const float m_Year = 1/525960;	// using 365.25 day years
	public const float h_Year = 1/8766;		// using 365.25 day years
	public const float d_Year = 1/365.25f;
	public const float ka_Year = 1000;
	public const float ma_Year = 1000000;
	public const float ga_Year = 1000000000;

	// Luminosity
	public const float solarLuminosity_Watt = 382800000000000000000000000f;

	/*
	m
	km				// BASE
	Earth Radius
	Solar Radius
	AU

	kg				// BASE
	Earth Mass
	Solar Mass

	Celsius
	Fahrenheit
	Kelvin			// BASE

	Second
	Minute
	Hour
	Earth Day
	Earth Year		// BASE
	Thousand Years
	Million Years
	Billion Years

	Watt			// BASE
	Solar Luminosity
	*/
}