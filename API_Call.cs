using System;
using Terraria;


namespace Licenses {
	public static partial class LicensesAPI {
		internal static object Call( string call_type, params object[] args ) {
			Item item;
			bool play_sound;

			switch( call_type ) {
			case "GetModSettings":
				return LicensesAPI.GetModSettings();
			case "SaveModSettingsChanges":
				LicensesAPI.SaveModSettingsChanges();
				return null;

			case "LicenseItemForCurrentPlayer":
				if( args.Length < 2 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				item = args[0] as Item;
				if( item == null ) { throw new Exception( "Invalid parameter 'item' for API call " + call_type ); }

				if( !( args[1] is bool ) ) { throw new Exception( "Invalid parameter 'play_sound' for API call " + call_type ); }
				play_sound = (bool)args[1];

				LicensesAPI.LicenseItemForCurrentPlayer( item, play_sound );
				return null;

			case "TrialLicenseItemForCurrentPlayer":
				if( args.Length < 2 ) { throw new Exception( "Insufficient parameters for API call " + call_type ); }

				item = args[0] as Item;
				if( item == null ) { throw new Exception( "Invalid parameter 'item' for API call " + call_type ); }

				if( !( args[1] is bool ) ) { throw new Exception( "Invalid parameter 'play_sound' for API call " + call_type ); }
				play_sound = (bool)args[1];

				LicensesAPI.TrialLicenseItemForCurrentPlayer( item, play_sound );
				return null;

			case "ClearLicencesForCurrentPlayer":
				LicensesAPI.ClearLicencesForCurrentPlayer();
				return null;
			}

			throw new Exception( "No such api call " + call_type );
		}
	}
}
