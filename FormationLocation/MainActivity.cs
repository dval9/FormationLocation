using Android.App;
using Android.Widget;
using Android.OS;
using Android.Locations;
using Android.Util;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System;

/***************************************************************************************************
 *https://developer.xamarin.com/recipes/android/os_device_resources/gps/get_current_device_location/
 * https://developer.xamarin.com/guides/android/deployment,_testing,_and_metrics/release-prep/
 **************************************************************************************************/

namespace FormationLocation
{
    [Activity(Label = "FormationLocation", MainLauncher = true)]
    public class MainActivity : Activity, ILocationListener
    {
        static readonly string TAG = "X:" + typeof(MainActivity).Name;
        Location _currentLocation;
        LocationManager _locationManager;
        string _locationProvider;

        Spinner spinner;
        ImageView form_pic;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            FindViewById<TextView>(Resource.Id.getGPSButton).Click += GetGPSButton_OnClick;

            spinner = FindViewById<Spinner>(Resource.Id.area_dropdown);
            spinner.ItemSelected += (sender, args) =>
            {
                switch (args.Position)
                {
                    case 1:
                        form_pic.SetImageResource(Resource.Drawable.nmf);
                        break;
                    case 2:
                        form_pic.SetImageResource(Resource.Drawable.cmf);
                        break;
                    case 3:
                        form_pic.SetImageResource(Resource.Drawable.smf);
                        break;
                    case 4:
                        form_pic.SetImageResource(Resource.Drawable.nwp);
                        break;
                    case 5:
                        form_pic.SetImageResource(Resource.Drawable.nep);
                        break;
                    case 6:
                        form_pic.SetImageResource(Resource.Drawable.wcp);
                        break;
                    case 7:
                        form_pic.SetImageResource(Resource.Drawable.ecp);
                        break;
                    case 8:
                        form_pic.SetImageResource(Resource.Drawable.sp);
                        break;
                    default:
                        form_pic.SetImageResource(Resource.Drawable.tof);
                        break;
                }
            };

            form_pic = FindViewById<ImageView>(Resource.Id.formations_pic);
            form_pic.SetImageResource(Resource.Drawable.tof);
            InitializeLocationManager();
        }

        void GetGPSButton_OnClick(object sender, EventArgs eventArgs)
        {
            if (_currentLocation == null)
            {
                spinner.SetSelection(0);
            }
            else
            {
                if (_currentLocation.Latitude > 54)
                {
                    if (_currentLocation.Longitude < -115)
                    {
                        spinner.SetSelection(4);
                    }
                    else
                    {
                        spinner.SetSelection(5);
                    }
                }
                else if (_currentLocation.Latitude > 51.5)
                {
                    if (_currentLocation.Longitude > -114)
                    {
                        spinner.SetSelection(7);
                    }
                    else if (_currentLocation.Longitude > -117)
                    {
                        spinner.SetSelection(6);
                    }
                    else
                    {
                        spinner.SetSelection(1);
                    }
                }
                else
                {
                    if (_currentLocation.Longitude > -114)
                    {
                        spinner.SetSelection(8);
                    }
                    else
                    {
                        if (_currentLocation.Latitude > 50.5)
                        {
                            spinner.SetSelection(2);
                        }
                        else
                        {
                            spinner.SetSelection(3);
                        }
                    }
                }
            }
        }

        void InitializeLocationManager()
        {
            _locationManager = (LocationManager)GetSystemService(LocationService);
            Criteria criteria = new Criteria
            {
                Accuracy = Accuracy.Coarse
            };
            IList<string> acceptableLocationProviders = _locationManager.GetProviders(criteria, true);

            if (acceptableLocationProviders.Any())
            {
                _locationProvider = acceptableLocationProviders.First();
            }
            else
            {
                _locationProvider = string.Empty;
            }
            Log.Debug(TAG, "Using " + _locationProvider + ".");
        }

        public void OnLocationChanged(Location location)
        {
            _currentLocation = location;
        }

        public void OnProviderDisabled(string provider)
        {

        }

        public void OnProviderEnabled(string provider)
        {

        }

        public void OnStatusChanged(string provider, Availability status, Bundle extras)
        {

        }

        protected override void OnResume()
        {
            base.OnResume();
            _locationManager.RequestLocationUpdates(_locationProvider, 0, 0, this);
        }

        protected override void OnPause()
        {
            base.OnPause();
            _locationManager.RemoveUpdates(this);
        }
    }
}

