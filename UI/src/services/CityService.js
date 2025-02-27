import ApiPaths from "../statics/ApiPaths";
import useApi from "../utils/apiUtils";

const useCityService = () => {
  const api = useApi();

  const getCities = async () => {
    const cities = await api.get(ApiPaths.GetCities);
    return cities;
  };

  const getCitiesByCountyId = async (countyId) => {
    const citiesByCoiuntyId = await api.get(ApiPaths.GetCitiesByCountyId(countyId));
    return citiesByCoiuntyId;
  };

  return {
    getCities,
    getCitiesByCountyId,
  };
};

export default useCityService;
