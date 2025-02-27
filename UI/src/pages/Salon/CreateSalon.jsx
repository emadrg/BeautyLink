import { Stack } from "@mui/material";
import { useEffect, useState } from "react";
import { MapContainer, Marker, TileLayer, useMapEvent } from "react-leaflet";
import { Select } from "../../components/widgets/Select";
import TextInput from "../../components/widgets/TextInput";
import useCityService from "../../services/CityService";
import useCountyService from "../../services/CountyService";
import userSession from "../../utils/userSession";

const CreateSalon = ({ salon, setSalon }) => {
  const [countyList, setCountyList] = useState([]);
  const [cityList, setCityList] = useState([]);
  const [countyIsSelected, setCountyIsSelected] = useState(false);
  const { getCounties } = useCountyService();
  const { getCitiesByCountyId } = useCityService();

  const user = userSession.user();
  const [currentPosition, setCurrentPosition] = useState({
    lat: 44.4520801,
    lng: 26.0873667,
  });
  const [salonPosition, setSalonPosition] = useState(undefined);

  useEffect(() => {
    // navigator.geolocation.getCurrentPosition((position) => {
    //   setCurrentPosition({
    //     lat: position.coords.latitude,
    //     lng: position.coords.longitude,
    //   });
    // });
  }, []);

  const handleCountySelect = (event) => {
    setSalon({ ...salon, county: event.target.value });
    setCountyIsSelected(true);
    handleCountyChange(event.target.value);
    fetchCities(event.target.value);
  };

  const handleCitySelect = (event) => {
    setSalon({ ...salon, city: event.target.value });
  };

  const fetchCounties = async () => {
    const counties = await getCounties();
    setCountyList(counties);
  };
  const fetchCities = async (countyId) => {
    const citiesByCountyId = await getCitiesByCountyId(countyId);
    setCityList(citiesByCountyId);
  };

  const getCitiesByCounty = async (countyId) => {
    const citiesByCountyId = await getCitiesByCountyId(countyId);
    setCityList(citiesByCountyId);
  };

  const handleCountyChange = async (event) => {
    setCityList(event.target.value);
    await getCitiesByCounty(event.county.value);
  };

  useEffect(() => {
    fetchCounties();
  }, []);

  return (
    <div className="create-salon-card">
      <Stack gap={3}>
        <div>
          <TextInput
            label={"Salon name"}
            variant="standard"
            name="name"
            value={salon.name}
            onChange={(e) => setSalon({ ...salon, name: e.target.value })}
          ></TextInput>
        </div>

        <Select
          options={countyList}
          value={salon.county}
          label={"Counties"}
          fullWidth
          onChange={handleCountySelect}
        ></Select>

        {countyIsSelected && (
          <Select
            options={cityList}
            value={salon.city}
            label={"Cities"}
            onChange={handleCitySelect}
          ></Select>
        )}

        <div>
          <TextInput
            label={"Salon address"}
            variant="standard"
            name="address"
            value={salon.address}
            onChange={(e) => setSalon({ ...salon, address: e.target.value })}
          ></TextInput>
        </div>
        <div>Select the position of the salon:</div>
        <div>
          {currentPosition && (
            <MapContainer
              center={currentPosition}
              zoom={13}
              style={{ height: 400, width: 400 }}
            >
              <TileLayer
                attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                url="https://www.google.cn/maps/vt?lyrs=m@189&gl=cn&x={x}&y={y}&z={z}"
              />
              {salonPosition && <Marker position={salonPosition}></Marker>}
              <MapEventContainer
                setSalon={setSalon}
                setSalonPosition={setSalonPosition}
              />
            </MapContainer>
          )}
        </div>
      </Stack>
    </div>
  );

  function MapEventContainer({ setSalon, setSalonPosition }) {
    useMapEvent("click", (e) => {
      const { lat, lng } = e.latlng;

      setSalon((prevSalon) => ({
        ...prevSalon,
        latitude: lat,
        longitude: lng,
      }));

      setSalonPosition({ lat: e.latlng.lat, lng: e.latlng.lng });
    });

    return null;
  }
};

export default CreateSalon;
