import * as React from "react";
import mapboxgl from "mapbox-gl";
import "mapbox-gl/dist/mapbox-gl.css"; 

interface MapboxMapProps {
  routeCoordinates: number[][];
}

const MapboxMap: React.FC<MapboxMapProps> = ({ routeCoordinates }) => {
  const [map, setMap] = React.useState<mapboxgl.Map>();

  const mapNode = React.useRef(null);

  React.useEffect(() => {
    const node = mapNode.current;
    if (typeof window === "undefined" || node === null) return;

    const mapboxMap = new mapboxgl.Map({
      container: node,
            accessToken: "pk.eyJ1IjoibG9zZHJldyIsImEiOiJjbHB1eGJkaHgwMHljMmtxeng2NzA4dndxIn0.S4r1YfGRASP85mHPYNjZuQ",
            style: "mapbox://styles/mapbox/streets-v11",
      center: [35.871651, 48.531681],
      zoom: 12,
    });

    setMap(mapboxMap);
    
    mapboxMap.on('load', () => {
      mapboxMap.addSource('route', {
        type: 'geojson',
        data: {
          type: 'Feature',
          properties: {},
          geometry: {
            type: 'LineString',
            coordinates: routeCoordinates
          },
        },
      });

      mapboxMap.addLayer({
        id: 'route',
        type: 'line',
        source: 'route',
        layout: {
          'line-join': 'round',
          'line-cap': 'round',
        },
        paint: {
          'line-color': '#536ee6',
          'line-width': 8,
        },
      });
    });

    return () => {
      mapboxMap.remove();
    };
  }, []);

  return ( 
    <div ref={mapNode} style={{ width: "500px", height: "500px" }}/>
  );
};

export default MapboxMap;