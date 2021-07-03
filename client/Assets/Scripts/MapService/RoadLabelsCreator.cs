﻿using UnityEngine;
using Google.Maps.Event;
using Google.Maps.Examples.Shared;


[RequireComponent(typeof(MapLabeller))]
public class RoadLabelsCreator : MonoBehaviour {
  /// <summary>
  /// The Labeller used to create road labels.
  /// </summary>
  private MapLabeller Labeller;

  public float Ratio = 0.1f;

  void Awake() {
    Labeller = GetComponent<MapLabeller>();
  }

  /// <summary>
  /// Add listeners for new road segment creations.
  /// </summary>
  void OnEnable() {
    Labeller.MapController.MapsService.Events.SegmentEvents.DidCreate.AddListener(
        OnSegmentCreated);
  }

  /// <summary>
  /// Remove listeners for new road segment creations.
  /// </summary>
  void OnDisable() {
    Labeller.MapController.MapsService.Events.SegmentEvents.DidCreate.RemoveListener(
        OnSegmentCreated);
  }

  /// <summary>
  /// Create a new road name label when a <see cref="Google.Maps.Feature.Segment"/> GameObject is
  /// created while loading the map.
  /// </summary>
  void OnSegmentCreated(DidCreateSegmentArgs args)
  {
    if (Random.value > Ratio) return;
    
    if (!Labeller.enabled)
      return;

    Label label = Labeller.NameObject(
        args.GameObject, args.MapFeature.Metadata.PlaceId, args.MapFeature.Metadata.Name);

    if (label != null) {
      // Get transformer component for re-positioning the label after road segment updates.
      RoadLabelMover roadLabelMover = label.GetComponent<RoadLabelMover>();
      if (roadLabelMover == null) {
        roadLabelMover = label.gameObject.AddComponent<RoadLabelMover>();
      }

      // Add road chunk to transformer.
      roadLabelMover.Add(args.GameObject, args.MapFeature.Shape.Lines[0]);
    }
  }
}
