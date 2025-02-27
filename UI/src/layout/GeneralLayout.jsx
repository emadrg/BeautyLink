import { Stack } from "@mui/material";
import { Outlet } from "react-router-dom";
import Footer from "../components/infrastructure/Footer";
import Navbar from "../components/infrastructure/Navbar";

const GeneralLayout = () => {
  return (
    <Stack position={"relative"} height={"100vh"}>
      <Navbar />
      <Stack marginX={"5rem"}>
        <Outlet />
      </Stack>
      <Footer />
    </Stack>
  );
};

export default GeneralLayout;
