import ApiPaths from "../statics/ApiPaths";
import useApi from "../utils/apiUtils";

const useCountyService = () => {
  const api = useApi();
  const getCounties = async () => {
    const counties = await api.get(ApiPaths.GetCounties);
    return counties;
  };

  return {
    getCounties,
  };
};

export default useCountyService;
