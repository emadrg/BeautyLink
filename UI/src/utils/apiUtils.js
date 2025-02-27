import axios from "axios";
import { useNavigate } from "react-router-dom";
import userSession from "./userSession";

const useApi = () => {
  const navigate = useNavigate();

  const call = async (method, path, payload, additionalHeaders) => {
    const url = `${import.meta.env.VITE_API_URL}${path}`;
    const headers = {
      "Content-Type":
        additionalHeaders != undefined ? additionalHeaders : "application/json",
    };

    if (userSession.token()) {
      headers.Authorization = `Bearer ${userSession.token()}`;
    }

    const options = {
      method: method,
      headers: headers,
    };

    if (method === "POST" && payload) {
      options.data = payload;
    }

    try {
      const response = await axios({
        url: url,
        ...options,
      });

      return response.data;
    } catch (error) {
      if (error.response) {
        console.error(`${error.response.status}: ${error.response.statusText}`);
        switch (error.response.status) {
          case 401:
          case 403:
            userSession.clearAuthSession();
            navigate("/login");
            break;
          // Add more cases as needed
          case 422:
            throw {
              status: error.response.status,
              message: error.response.data || "An error occurred",
            };
          default:
            navigate("/error", {
              state: {
                status: error.response.status,
                message: error.response.data.message || "An error occurred",
              },
            });
            break;
        }
      } else {
        console.error("Error: ", error.message);
        navigate("/error", {
          state: {
            status: "Unknown",
            message: error.message || "An error occurred",
          },
        });
      }
      throw error;
    }
  };

  return {
    get: async (path, payload, additionalHeaders) => {
      return await call("GET", path, payload, additionalHeaders);
    },
    post: async (path, payload, additionalHeaders) => {
      return await call("POST", path, payload, additionalHeaders);
    },
    put: async (path, payload, additionalHeaders) => {
      return await call("PUT", path, payload, additionalHeaders);
    },
    delete: async (path, payload, additionalHeaders) => {
      return await call("DELETE", path, payload, additionalHeaders);
    },
  };
};

export default useApi;
