import { Box, Rating, Stack } from "@mui/material";
import { AdapterDayjs } from "@mui/x-date-pickers/AdapterDayjs";
import { DateTimePicker } from "@mui/x-date-pickers/DateTimePicker";
import { DemoContainer } from "@mui/x-date-pickers/internals/demo";
import { LocalizationProvider } from "@mui/x-date-pickers/LocalizationProvider";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Select } from "../../components/widgets/Select";
import useCityService from "../../services/CityService";
import useCountyService from "../../services/CountyService";
import useServiceEntityService from "../../services/ServiceEntityService";
import useStylistService from "../../services/StylistService";
import { getFile } from "../../utils";

const SearchStylists = () => {
  const [countyList, setCountyList] = useState([]);
  const [cityList, setCityList] = useState([]);
  const [countyIsSelected, setCountyIsSelected] = useState(false);
  const [cityIsSelected, setCityIsSelected] = useState(false);
  const [serviceIsSelected, setServiceIsSelected] = useState(false);
  const [cityId, setCityId] = useState(0);
  const [isLoading, setIsLoading] = useState(true);
  const [serviceList, setserviceList] = useState([]);
  const [service, setService] = useState("");
  const [availableTimeStartTime, setAvailableTimeStartTime] =
    useState(undefined);
  const [availableTimeEndTime, setAvailableTimeEndTime] = useState(undefined);
  const [filteredStylists, setFilteredStylists] = useState(undefined);
  const [stylistsAreLoading, setStylistsAreLoading] = useState(true);

  const { getStylistByFiltering } = useStylistService();
  const { getCounties } = useCountyService();
  const { getCitiesByCountyId } = useCityService();
  const { getServices } = useServiceEntityService();
  const navigate = useNavigate();

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

  const handleCountySelect = (event) => {
    setCountyIsSelected(true);
    handleCountyChange(event.target.value);
    fetchCities(event.target.value);
    setServiceIsSelected(false);
  };

  const handleCitySelect = (event) => {
    setCityId(event.target.value);
    setCityIsSelected(true);
    setService(undefined);
    setServiceIsSelected(false);
  };

  const handleServiceChange = async (event) => {
    setService(event.target.value);
    setServiceIsSelected(true);
  };

  const handleSearchStylists = async () => {
    setStylistsAreLoading(true);
    try {
      const addHours = (n) => n * 60 * 60 * 1000;
      const stylists = await getStylistByFiltering(
        new Date(availableTimeStartTime.$d.getTime() + addHours(3)).toJSON(),
        new Date(availableTimeEndTime.$d.getTime() + addHours(3)).toJSON(),
        service,
        cityId
      );
      debugger;
      setFilteredStylists(stylists);
      setStylistsAreLoading(false);
    } catch (e) {
      setStylistsAreLoading(false);
    }
  };

  useEffect(() => {
    fetchCounties();
    fetchServices();
  }, []);

  return (
    <div>
      <Stack gap={3}>
        <div>
          <Select
            options={countyList}
            label={"Where?"}
            onChange={handleCountySelect}
            sx={{ minWidth: 300, marginTop: 3 }}
          ></Select>
        </div>

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
        {serviceIsSelected && (
          <LocalizationProvider dateAdapter={AdapterDayjs}>
            <DemoContainer components={["DateTimePicker", "DateTimePicker"]}>
              <DateTimePicker
                label="Start of your available interval"
                value={availableTimeStartTime}
                onChange={(val) => setAvailableTimeStartTime(val)}
              />
              <DateTimePicker
                label="End of your available interval"
                value={availableTimeEndTime}
                onChange={(val) => setAvailableTimeEndTime(val)}
              />
            </DemoContainer>
          </LocalizationProvider>
        )}
      </Stack>
      {availableTimeEndTime && stylistsAreLoading && (
        <button
          style={{ marginTop: 10 }}
          onClick={() => handleSearchStylists()}
        >
          Search stylists
        </button>
      )}
      {!stylistsAreLoading && filteredStylists.length == 0 && (
        <div>No stylist available for your selection</div>
      )}
      {!stylistsAreLoading && filteredStylists.length > 0 && (
        <div className="all-returned-stylists">
          {filteredStylists.map((stylist) => (
            <div key={stylist.id}>
              <div
                className="stylist-card-for-filter"
                style={{ display: "flex" }}
              >
                <div style={{ display: "block" }}>
                  <div
                    className="stylist-card-for-manager-name"
                    onClick={() => navigate(`/appointment/${stylist.id}`)}
                  >
                    {stylist.firstName}
                    {stylist.lastName}
                  </div>
                  <div
                    className="stylist-card-for-manager-name"
                    onClick={() => navigate(`/salons/${stylist.salonId}`)}
                  >
                    {""} {stylist.salon}
                  </div>
                  <div>
                    <Box component="fieldset" mb={3} borderColor="transparent">
                      <Rating
                        name="read-only"
                        value={stylist.averageScore}
                        readOnly
                      />
                    </Box>
                    {""}
                  </div>{" "}
                </div>
                <div className="stylist-card-for-manager-image">
                  <img
                    src={getFile(stylist.profilePicture)}
                    style={{
                      width: 50,
                      height: 50,
                      display: "cover",
                      borderRadius: "50%",
                      marginRight: 10,
                    }}
                  />
                </div>
              </div>
            </div>
          ))}
        </div>
      )}
    </div>
  );
};

export default SearchStylists;
