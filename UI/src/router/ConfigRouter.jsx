import { createBrowserRouter } from "react-router-dom";
import GeneralLayout from "../layout/GeneralLayout";
import Login from "../pages/Auth/Login";
import Home from "../pages/Home/Home";
import NotFound from "../pages/NotFound";
import Paths from "../statics/Paths";

import AuthenticatedRoute from "../components/infrastructure/AuthenticatedRoute";
import Register from "../pages/Auth/Register";
import AppointmentDetails from "../pages/ClientPages/AppointmentDetails";
import ClientDetailsDisplayReviews from "../pages/ClientPages/ClientDetailsDisplayReviews";
import ClientDetailsForStylist from "../pages/ClientPages/ClientDetailsForStylist";
import ClientReceivedAndLeftReviews from "../pages/ClientPages/ClientReceivedAndLeftReviews";
import SearchStylists from "../pages/ClientPages/SearchStylists";
import ComponentsStyles from "../pages/ComponentsStyles";
import DemoPage from "../pages/DemoPage";
import ErrorPage from "../pages/ErrorPage";
import SalonAllReviews from "../pages/Manager/SalonAllReviews";
import ReviewSuccessfullySent from "../pages/Review/ReviewSuccessfullySent";
import AllSalons from "../pages/Salon/AllSalons";
import SalonDetails from "../pages/Salon/SalonDetails";
import Appointment from "../pages/Stylist/Appointment/Appointment";
import AppointmentSuccessfullyCreated from "../pages/Stylist/Appointment/AppointmentSuccessfullyCreated";
import StylistAppointmentDetails from "../pages/Stylist/AppointmentDetails";
import EditOrAddServices from "../pages/Stylist/EditOrAddServices";
import Schedule from "../pages/Stylist/Schedule";
import StylistPresentationPage from "../pages/Stylist/StylistPresentationPage";
import StylistPresentationPageForManager from "../pages/Stylist/StylistPresentationPageForManager";
import StylistReceivedAndLeftReviews from "../pages/Stylist/StylistReceivedAndLeftReviews";

export default function configRouter() {
  return createBrowserRouter([
    {
      path: Paths.home,
      element: <GeneralLayout />,
      children: [
        {
          path: Paths.home,
          element: <Home />,
        },
        {
          path: Paths.login,
          element: <Login />,
        },
        {
          path: Paths.register,
          element: <Register />,
        },
        {
          path: Paths.salons,
          element: <AllSalons />,
        },
        {
          path: Paths.demo,
          element: <DemoPage />,
        },
        {
          path: Paths.components,
          element: <ComponentsStyles />,
        },
        {
          path: Paths.users,
          element: (
            <AuthenticatedRoute>
              <Home />
            </AuthenticatedRoute>
          ),
        },
        {
          path: Paths.error,
          element: <ErrorPage />,
        },
        {
          path: Paths.salon,
          element: <SalonDetails />,
        },
        {
          path: Paths.stylist,
          element: <StylistPresentationPage />,
        },
        {
          path: Paths.appointment,
          element: <Appointment />,
        },
        {
          path: Paths.appointmentSuccessfullyCreated,
          element: <AppointmentSuccessfullyCreated />,
        },
        {
          path: Paths.reviewSuccessfullySent,
          element: <ReviewSuccessfullySent />,
        },
        {
          path: Paths.clientDetailsForStylist,
          element: <ClientDetailsForStylist />,
        },

        {
          path: Paths.ClientDetailsDisplayReviews,
          element: <ClientDetailsDisplayReviews />,
        },
        {
          path: Paths.StylistReceivedAndLeftReviews,
          element: <StylistReceivedAndLeftReviews />,
        },
        {
          path: Paths.ClientReceivedAndLeftReviews,
          element: <ClientReceivedAndLeftReviews />,
        },
        {
          path: Paths.schedule,
          element: <Schedule />,
        },
        {
          path: Paths.stylistPresentationPageForManager,
          element: <StylistPresentationPageForManager />,
        },
        {
          path: Paths.salonAllReviews,
          element: <SalonAllReviews />,
        },
        {
          path: Paths.clientAllAppointments,
          element: <AppointmentDetails />,
        },
        {
          path: Paths.stylistAllAppointments,
          element: <StylistAppointmentDetails />,
        },
        {
          path: Paths.searchStylists,
          element: <SearchStylists />,
        },
        {
          path: Paths.editOrAddServices,
          element: <EditOrAddServices />,
        },
        {
          path: "*",
          element: <NotFound />,
        },
      ],
    },
  ]);
}
