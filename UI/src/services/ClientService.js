import ApiPaths from "../statics/ApiPaths";
import useApi from "../utils/apiUtils";

const useClientService = () => {
  const api = useApi();

  const getClientDetailsById = async (clientId) => {
    const client = await api.get(ApiPaths.GetClientDetailsByClientId(clientId));
    return client;
  };
  return {
    getClientDetailsById,
  };
};

export default useClientService;
