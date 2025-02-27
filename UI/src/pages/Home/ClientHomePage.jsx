import { Stack } from "@mui/material";
import { useEffect, useState } from "react";
import {
  MapContainer,
  Marker,
  Popup,
  TileLayer,
  useMapEvent,
} from "react-leaflet";
import { useNavigate } from "react-router-dom";
import "../../../src/styles/base/base.scss";
import "../../../src/styles/pages/clientHomePage.scss";
import { Select } from "../../components/widgets/Select";
import useCityService from "../../services/CityService";
import useCountyService from "../../services/CountyService";
import useSalonService from "../../services/SalonService";
import useServiceEntityService from "../../services/ServiceEntityService";
import userSession from "../../utils/userSession";
import SearchStylists from "../ClientPages/SearchStylists";
import SalonListItem from "../Salon/SalonListItem";
export default function ClientHomePage() {
  const user = userSession.user();
  const [currentPosition, setCurrentPosition] = useState({
    lat: 44.4520801,
    lng: 26.0873667,
  });
  const [salons, setSalons] = useState([]);
  const [service, setService] = useState("");
  const [serviceList, setserviceList] = useState([]);
  const [countyList, setCountyList] = useState([]);
  const [cityList, setCityList] = useState([]);
  const [countyIsSelected, setCountyIsSelected] = useState(false);
  const [cityIsSelected, setCityIsSelected] = useState(false);
  const [cityId, setCityId] = useState(0);
  const [salonsByCity, setSalonsByCity] = useState([]);
  const [lastVisitedSalon, setLastVisitedSalon] = useState(undefined);
  const [isLoading, setIsLoading] = useState(true);
  const [suggestionsLoading, setSuggestionsLoading] = useState(true);
  const [salonSuggestion, setSalonSuggestion] = useState(undefined);
  const [searchByStylistButtonIsPressed, setSearchByStylistButtonIsPressed] =
    useState(false);
  const [inputValue, setInputValue] = useState(null);

  const { getCounties } = useCountyService();
  const { getCitiesByCountyId } = useCityService();
 
  const {
    getSalons,
    getSalonsByCityIdAndServiceId,
    getLastVisitedSalonByClientId,
    getSalonSuggestions,
  } = useSalonService();
  const { getServices } = useServiceEntityService();

  const navigate = useNavigate();

  const fetchLastVisitedSalon = async () => {
    setIsLoading(true);
    try {
      let salon = await getLastVisitedSalonByClientId(user.id);
      setLastVisitedSalon(salon);
      setIsLoading(false);
      fetchSalonSuggestions(salon.id);
    } catch (e) {
      setIsLoading(false);
    }
  };

  const fetchSalonSuggestions = async (lastVisitedSalonId) => {
    setSuggestionsLoading(true);
    try {
      let suggestions = await getSalonSuggestions(lastVisitedSalonId);
      setSalonSuggestion(suggestions);
      setSuggestionsLoading(false);
    } catch (e) {
      setSuggestionsLoading(false);
    }
  };

  const fetchSalons = async () => {
    setIsLoading(true);
    try {
      const salons = await getSalons(null, 0, 20);
      setSalons(salons);
      setIsLoading(false);
    } catch (e) {
      setIsLoading(false);
    }
  };

  const fetchSalonsByCity = async (cityId, service) => {
    setIsLoading(true);
    try {
      const salonsByCity = await getSalonsByCityIdAndServiceId(cityId, service);
      setSalonsByCity(salonsByCity);
      setIsLoading(false);
    } catch (e) {
      setIsLoading(false);
    }
  };

  const fetchServices = async () => {
    setIsLoading(true);
    try {
      const services = await getServices();
      setserviceList(services);
      setIsLoading(false);
    } catch (e) {
      setIsLoading(false);
    }
  };

  const handleCountySelect = (event) => {
    setCountyIsSelected(true);
    handleCountyChange(event.target.value);
    fetchCities(event.target.value);
  };

  const handleCitySelect = (event) => {
    setCityId(event.target.value);
    setCityIsSelected(true);
    setService(undefined);
  };

  const handleServiceChange = async (event) => {
    setService(event.target.value);
  };

  const fetchCounties = async () => {
    setIsLoading(true);
    try {
      setIsLoading(false);
      const counties = await getCounties();
      setCountyList(counties);
    } catch (e) {
      setIsLoading(false);
    }
  };

  const fetchCities = async (countyId) => {
    setIsLoading(true);
    try {
      const citiesByCountyId = await getCitiesByCountyId(countyId);
      setCityList(citiesByCountyId);
      setIsLoading(false);
    } catch (e) {
      setIsLoading(false);
    }
  };

  const getCitiesByCounty = async (countyId) => {
    setIsLoading(true);
    try {
      setIsLoading(false);
      const citiesByCountyId = await getCitiesByCountyId(countyId);
      setCityList(citiesByCountyId);
    } catch (e) {
      setIsLoading(false);
    }
  };

  const handleCountyChange = async (event) => {
    setCityList(event.target.value);
    await getCitiesByCounty(event.county.value);
  };

  const navigateToAllSalonsPage = () => {
    navigate("/salons");
  };

  const handleSearchByStylistButtonPress = () => {
    setSearchByStylistButtonIsPressed(!searchByStylistButtonIsPressed);
    debugger;
  };

  useEffect(() => {
    navigator.geolocation.getCurrentPosition((position) => {
      setCurrentPosition({
        lat: position.coords.latitude,
        lng: position.coords.longitude,
      });
    });
    fetchSalons();
    fetchCounties();
    fetchLastVisitedSalon();
    fetchServices();
  }, []);

  useEffect(() => {
    fetchSalonsByCity(cityId, service);
  }, [cityId, service]);

  return (
    <div className="client-homepage">
      <div className="salon-cards">
        <div className="dropdown-homepage-client">
          {!searchByStylistButtonIsPressed && (
            <div>
              <Select
                options={countyList}
                label={"Where?"}
                onChange={handleCountySelect}
                sx={{ minWidth: 300 }}
              ></Select>

              {countyIsSelected && (
                <div>
                  <Select
                    options={cityList}
                    label={"What city?"}
                    onChange={handleCitySelect}
                    sx={{ minWidth: 200 }}
                  ></Select>
                </div>
              )}
              {cityIsSelected && (
                <div>
                  <Select
                    options={serviceList}
                    label={"Service"}
                    onChange={handleServiceChange}
                    sx={{ minWidth: 300 }}
                  ></Select>
                </div>
              )}
            </div>
          )}

          {!searchByStylistButtonIsPressed && (
            <div>
              &nbsp; or &nbsp;
              <button onClick={handleSearchByStylistButtonPress}>
                Search by stylist
              </button>
            </div>
          )}

          {searchByStylistButtonIsPressed && <SearchStylists />}
        </div>
        {searchByStylistButtonIsPressed && (
          <button onClick={handleSearchByStylistButtonPress}>
            Search by salon
          </button>
        )}

        {salonsByCity.length == 0 && cityIsSelected && (
          <div>No salons match your search</div>
        )}
        <Stack gap={0}>
          {salonsByCity.length != 0 && (
            <div className="salon-list">
              {salonsByCity.map((salonByCity) => {
                return (
                  <SalonListItem
                    key={salonByCity.id}
                    id={salonByCity.id}
                    name={salonByCity.name}
                    county={salonByCity.county}
                    city={salonByCity.city}
                    address={salonByCity.address}
                  ></SalonListItem>
                );
              })}
            </div>
          )}
        </Stack>

        {!isLoading && lastVisitedSalon && lastVisitedSalon.county != null && (
          <div>
            {" "}
            <h2>Last visited salon:</h2>
            <SalonListItem
              key={lastVisitedSalon.id}
              id={lastVisitedSalon.id}
              name={lastVisitedSalon.name}
              county={lastVisitedSalon.county}
              city={lastVisitedSalon.city}
              address={lastVisitedSalon.address}
            ></SalonListItem>
          </div>
        )}
        {!isLoading && lastVisitedSalon && lastVisitedSalon.county == null && (
          <div>No visited salons yet</div>
        )}

        {salonSuggestion && !suggestionsLoading && (
          <div>
            {" "}
            <h2>Salon suggestions:</h2>
            <div className="salon-suggestions">
              {salonSuggestion.map((salonSuggestion) => {
                return (
                  <div key={salonSuggestion.id}>
                    <>
                      {" "}
                      <SalonListItem
                        id={salonSuggestion.id}
                        name={salonSuggestion.name}
                        city={salonSuggestion.city}
                        county={salonSuggestion.county}
                        address={salonSuggestion.address}
                      ></SalonListItem>
                    </>
                  </div>
                );
              })}
            </div>
          </div>
        )}

        {salonSuggestion &&
          salonSuggestion.length == 0 &&
          !suggestionsLoading && <div>No suggestions for you yet</div>}
      </div>
      <div className="map">
        {" "}
        <h2>Salons near you:</h2>
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
            <Marker position={currentPosition}>
              <Popup>Your current location</Popup>
            </Marker>

            {cityIsSelected && (
              <div>
                {salonsByCity.map((salon) => {
                  return (
                    <Marker
                      key={salon.id}
                      position={{ lat: salon.latitude, lng: salon.longitude }}
                    >
                      <Popup>{salon.name}</Popup>
                    </Marker>
                  );
                })}
              </div>
            )}

            {!cityIsSelected && (
              <div>
                {salons.map((salon) => {
                  return (
                    <Marker
                      key={salon.id}
                      position={{ lat: salon.latitude, lng: salon.longitude }}
                    >
                      <Popup>{salon.name}</Popup>
                    </Marker>
                  );
                })}
              </div>
            )}

            <MapEventContainer />
          </MapContainer>
        )}
        <button onClick={navigateToAllSalonsPage} style={{ marginTop: 5 }}>
          See All Salons
        </button>
      </div>
    </div>
  );
}

function MapEventContainer() {
  const map = useMapEvent("click", (e) => {});
  return null;
}
