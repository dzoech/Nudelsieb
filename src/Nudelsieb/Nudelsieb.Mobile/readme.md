# Nudelsieb.Mobile setup

## Building the Xamarin.Android app

Go to the [Firebase console](https://console.firebase.google.com/project/nudelsieb-mobile/settings/general/android:dev.zoechbauer.nudelsieb) and download the `google-services.json`. This contains `client` entries for `dev.zoechbauer.nudelsieb` as well as `dev.zoechbauer.nudelsieb.debug`. The latter is used to install debug builds from the IDE on your phone without replacing a release version installed from an APK.

Either copy your `google-services.json` into the project's root directory next to `Nudelsieb.Mobile.Android.csproj` or use the MSBuild argument `/p:GoogleServicesJsonFilePath=myFilePath` to read the file from a different location.

In `Nudelsieb.Mobile.Android.csproj` several variables are set depending if the build uses a Release or Debug configuration. These variables are then used in `Properties\AndroidManifest.xml`.