//-----------------------------------------------------------------------
// <copyright file="HelloARController.cs" company="Google LLC">
//
// Copyright 2020 Google LLC
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
// </copyright>
//-----------------------------------------------------------------------

namespace GoogleARCore.Examples.HelloAR
{
    using GoogleARCore;
    using GoogleARCore.Examples.Common;
    using UnityEngine;
    using UnityEngine.UI;
    using UnityEngine.EventSystems;

#if UNITY_EDITOR
    // Set up touch input propagation while using Instant Preview in the editor.
    using Input = InstantPreviewInput;
#endif

    /// <summary>
    /// Controls the HelloAR example.
    /// </summary>
    public class GameInitController : MonoBehaviour
    {
        /// <summary>
        /// The Depth Setting Menu.
        /// </summary>
        public DepthMenu DepthMenu;

        /// <summary>
        /// The Instant Placement Setting Menu.
        /// </summary>
        public InstantPlacementMenu InstantPlacementMenu;

        /// <summary>
        /// The first-person camera being used to render the passthrough camera image (i.e. AR
        /// background).
        /// </summary>
        public Camera FirstPersonCamera;

        /// <summary>
        /// A prefab to place when a raycast from a user touch hits a vertical plane.
        /// </summary>
        public GameObject GameObjectVerticalPlanePrefab;

        /// <summary>
        /// A prefab to place when a raycast from a user touch hits a horizontal plane.
        /// </summary>
        public GameObject GameObjectHorizontalPlanePrefab;

        /// <summary>
        /// A prefab to place when a raycast from a user touch hits a feature point.
        /// </summary>
        public GameObject GameObjectPointPrefab;

        /// <summary>
        /// A prefab to place when a raycast from a user touch hits a depth point.
        /// </summary>
        public GameObject GameObjectDepthPointPrefab;

        /// <summary>
        /// The rotation in degrees need to apply to prefab when it is placed.
        /// </summary>
        private const float _prefabRotation = 180.0f;

        /// <summary>
        /// True if the app is in the process of quitting due to an ARCore connection error,
        /// otherwise false.
        /// </summary>
        private bool _isQuitting = false;

        public Button placeGroundButton;
        public Button placeTombButton;
        public Button startGameButton;
        public GameObject dummyPrefab;
        public GameObject groundPlanePrefab;
        public GameObject tombPrefab;
        private GameObject groundPlaneGO;
        private GameObject dummyGO;
        private bool isDummyInitialized;
        private bool isGroundPlaced;
        private bool hasGameStarted;
        private int numOfTombsPlaced = 0;
        private GameObject dumm;
        private bool isPlacedDumm = false;
        public bool started = false;

        /// <summary>
        /// The Unity Awake() method.
        /// </summary>
        public void Awake()
        {
            // Enable ARCore to target 60fps camera capture frame rate on supported devices.
            // Note, Application.targetFrameRate is ignored when QualitySettings.vSyncCount != 0.
            Application.targetFrameRate = 60;
            placeTombButton.gameObject.SetActive(false);
            placeGroundButton.gameObject.SetActive(false);
            startGameButton.gameObject.SetActive(false);
        }

        /// <summary>
        /// The Unity Update() method.
        /// </summary>
        public void Update()
        {
            // To use Recording API:
            // 1. Create an instance of ARCoreRecordingConfig. The Mp4DatasetFilepath needs to
            // be accessible by the app, e.g. Application.persistentDataPath, or you can request
            // the permission of external storage.
            // 2. Call Session.StartRecording(ARCoreRecordingConfig) when a valid ARCore session
            // is available.
            // 3. Call Session.StopRecording() to end the recording. When
            // ARCoreRecordingConfig.AutoStopOnPause is true, it can also stop recording when
            // the ARCoreSession component is disabled.
            // To use Playback API:
            // 1. Pause the session by disabling ARCoreSession component.
            // 2. In the next frame or later, call Session.SetPlaybackDataset(datasetFilepath)
            // where the datasetFilepath is the same one used by Recording API.
            // 3. In the next frame or later, resume the session by enabling ARCoreSession component
            // and the app will play the recorded camera stream install of using the real time
            // camera stream.
            UpdateApplicationLifecycle();
            if (isPlacedDumm)
            {
                return;
            }
            // If the player has not touched the screen, we are done with this update.
            Touch touch;
            if (Input.touchCount < 1 || (touch = Input.GetTouch(0)).phase != TouchPhase.Began)
            {
                return;
            }

            // Should not handle input if the player is pointing on UI.
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return;
            }

            // Raycast against the location the player touched to search for planes.
            TrackableHit hit;
            bool foundHit = false;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinPolygon |
                TrackableHitFlags.FeaturePointWithSurfaceNormal;
            // Allows the depth image to be queried for hit tests.
            raycastFilter |= TrackableHitFlags.Depth;
            foundHit = Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit);
            if (!foundHit && InstantPlacementMenu.IsInstantPlacementEnabled())
            {
                foundHit = Frame.RaycastInstantPlacement(
                    touch.position.x, touch.position.y, 1.0f, out hit);
            }

            if (foundHit)
            {
                // Use hit pose and camera pose to check if hit test is from the
                // back of the plane, if it is, there is no need to create the anchor.
                if ((hit.Trackable is DetectedPlane) &&
                    Vector3.Dot(FirstPersonCamera.transform.position - hit.Pose.position,
                        hit.Pose.rotation * Vector3.up) < 0)
                {
                    Debug.Log("Hit at back of the current DetectedPlane");
                }
                else
                {
                    if (DepthMenu != null)
                    {
                        // Show depth card window if necessary.
                        DepthMenu.ConfigureDepthBeforePlacingFirstAsset();
                    }

                    // Choose the prefab based on the Trackable that got hit.
                    GameObject prefab;
                    if (hit.Trackable is InstantPlacementPoint)
                    {
                        prefab = dummyPrefab;
                    }
                    else if (hit.Trackable is FeaturePoint)
                    {
                        prefab = GameObjectPointPrefab;
                    }
                    else if (hit.Trackable is DepthPoint)
                    {
                        prefab = GameObjectDepthPointPrefab;
                    }
                    else if (hit.Trackable is DetectedPlane)
                    {
                        DetectedPlane detectedPlane = hit.Trackable as DetectedPlane;
                        if (detectedPlane.PlaneType == DetectedPlaneType.Vertical)
                        {
                            prefab = GameObjectVerticalPlanePrefab;
                        }
                        else
                        {
                            prefab = GameObjectHorizontalPlanePrefab;
                        }
                    }
                    else
                    {
                        prefab = GameObjectHorizontalPlanePrefab;
                    }

                    // Instantiate prefab at the hit pose.
                    dumm = Instantiate(prefab, hit.Pose.position, hit.Pose.rotation);

                    // Compensate for the hitPose rotation facing away from the raycast (i.e.
                    // camera).
                    dumm.transform.Rotate(0, _prefabRotation, 0, Space.Self);

                    // Create an anchor to allow ARCore to track the hitpoint as understanding of
                    // the physical world evolves.
                    var anchor = hit.Trackable.CreateAnchor(hit.Pose);

                    // Make game object a child of the anchor.
                    dumm.transform.parent = anchor.transform;

                    // Initialize Instant Placement Effect.
                    // if (hit.Trackable is InstantPlacementPoint)
                    // {
                    //     dumm.GetComponentInChildren<InstantPlacementEffect>()
                    //         .InitializeWithTrackable(hit.Trackable);
                    // }
                    isPlacedDumm = true;
                    placeGroundButton.gameObject.SetActive(true);
                    placeGroundButton.onClick.AddListener(PlaceGround);
                }
            }
        }

        /// <summary>
        /// Check and update the application lifecycle.
        /// </summary>
        private void UpdateApplicationLifecycle()
        {
            // Exit the app when the 'back' button is pressed.
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }

            // Only allow the screen to sleep when not tracking.
            if (Session.Status != SessionStatus.Tracking)
            {
                Screen.sleepTimeout = SleepTimeout.SystemSetting;
            }
            else
            {
                Screen.sleepTimeout = SleepTimeout.NeverSleep;
            }

            if (_isQuitting)
            {
                return;
            }

            // Quit if ARCore was unable to connect and give Unity some time for the toast to
            // appear.
            if (Session.Status == SessionStatus.ErrorPermissionNotGranted)
            {
                ShowAndroidToastMessage("Camera permission is needed to run this application.");
                _isQuitting = true;
                Invoke("DoQuit", 0.5f);
            }
            else if (Session.Status.IsError())
            {
                ShowAndroidToastMessage(
                    "ARCore encountered a problem connecting.  Please start the app again.");
                _isQuitting = true;
                Invoke("DoQuit", 0.5f);
            }
        }

        /// <summary>
        /// Actually quit the application.
        /// </summary>
        private void DoQuit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Show an Android toast message.
        /// </summary>
        /// <param name="message">Message string to show in the toast.</param>
        private void ShowAndroidToastMessage(string message)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject unityActivity =
                unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            if (unityActivity != null)
            {
                AndroidJavaClass toastClass = new AndroidJavaClass("android.widget.Toast");
                unityActivity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    AndroidJavaObject toastObject =
                        toastClass.CallStatic<AndroidJavaObject>(
                            "makeText", unityActivity, message, 0);
                    toastObject.Call("show");
                }));
            }
        }

        public void PlaceGround()
        {
            Vector3 pos = dumm.transform.position;
            Quaternion rotation = dumm.transform.rotation;
            groundPlaneGO = Instantiate(groundPlanePrefab, pos, rotation);
            Destroy(dumm);
            placeTombButton.gameObject.SetActive(true);
            placeTombButton.onClick.AddListener(PlaceTomb);
        }

        public void PlaceTomb()
        {
            Vector3 pos = FirstPersonCamera.transform.position;
            Instantiate(tombPrefab, pos, Quaternion.identity);
            numOfTombsPlaced++;
            if (numOfTombsPlaced == 5)
            {
                startGameButton.gameObject.SetActive(true);
                startGameButton.onClick.AddListener(delegate
                {
                    started = true;
                    placeTombButton.gameObject.SetActive(false);
                    startGameButton.gameObject.SetActive(false);
                });
            }
        }
    }
}