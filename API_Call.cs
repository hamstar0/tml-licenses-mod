using System;


namespace Licenses {
	public static partial class LicensesAPI {
		internal static object Call( string call_type, params object[] args ) {
			switch( call_type ) {
			case "GetModSettings":
				return LicensesAPI.GetModSettings();
			case "SaveModSettingsChanges":
				LicensesAPI.SaveModSettingsChanges();
				return null;
			}

			throw new Exception( "No such api call " + call_type );
		}
	}
}
