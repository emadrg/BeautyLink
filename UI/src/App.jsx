//import "leaflet/dist/leaflet.css";
import { RouterProvider } from "react-router-dom";
import CustomLoader from "./components/utility/CustomLoader";
import { useLoader } from "./contexts/LoaderContext";
import "./resources/config.js";
import configRouter from "./router/ConfigRouter";

export default function App() {
  const { loading } = useLoader();

  return loading ? (
    <CustomLoader />
  ) : (
    <RouterProvider router={configRouter()}></RouterProvider>
  );
}
