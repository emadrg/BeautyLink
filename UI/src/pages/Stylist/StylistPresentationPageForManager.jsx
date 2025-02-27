import interactionPlugin from "@fullcalendar/interaction";
import FullCalendar from "@fullcalendar/react";
import timeGridPlugin from "@fullcalendar/timegrid";
import { useEffect, useState } from "react";
import { useNavigate, useParams } from "react-router-dom";
import useAppointmentService from "../../services/AppointmentService";
import useClientStylistReviewService from "../../services/ClientStylistReviewService";
import useScheduleService from "../../services/ScheduleService";
import useStylistService from "../../services/StylistService";
import useUnavailableTimeServce from "../../services/UnavailableTimeServce";

import Stylist from "./Stylist";

const StylistPresentationPage = () => {
  const { id: stylistId } = useParams();

  const [reviews, setReviews] = useState([]);
  const [loading, setLoading] = useState(true);
  const { getClientStylistReviewsByStylistId } =
    useClientStylistReviewService();
  const [appointments, setAppointments] = useState([]);
  const [clickedAppointment, setClickedAppointment] = useState(undefined);
  const [clickedAppointmentIsLoading, setClickedAppointmentIsLoading] =
    useState(true);
  const [isLoading, setIsLoading] = useState(true);
  const [mergedArray, setMergedArray] = useState(undefined);
  const [schedule, setSchedule] = useState([]);
  const [unavailableTime, setUnavailableTime] = useState([]);
  const [appointmentsareLoading, setAppointmentsAreLoading] = useState(true);
  const { getStylistAppointments } = useStylistService();
  const {
    getAppointmentByStartDateAndStylistId,
    acceptAppointment,
    denyAppointment,
  } = useAppointmentService();
  const { getScheduleByStylistId } = useScheduleService();
  const { getUnavailableTimeByStylistId } = useUnavailableTimeServce();
  const navigate = useNavigate();
  const lightGrey = "#eeee";
  const green = "#9acc97";
  const red = "#d67676";

  const fetchReviews = async (stylistId) => {
    try {
      const fetchedReviews = await getClientStylistReviewsByStylistId(
        stylistId
      );
      setReviews(fetchedReviews);
      setLoading(false);
    } catch (e) {
      setLoading(false);
      console.log(e);
    }
  };

  useEffect(() => {
    fetchReviews(stylistId);

    fetchStylistAppointments();

    fetchSchedule();
    fetchUnavailableTime();
  }, []);

  const fetchSchedule = async () => {
    try {
      setSchedule(await getScheduleByStylistId(stylistId));
      setIsLoading(false);
    } catch (e) {
      console.log(e);
      setIsLoading(false);
    }
  };

  const fetchUnavailableTime = async () => {
    try {
      let nonWorkingIntervals = await getUnavailableTimeByStylistId(stylistId);

      setUnavailableTime(
        nonWorkingIntervals.map((interval) => ({
          start: interval.startDate,
          end: interval.endDate,
          color: lightGrey,
          textColor: lightGrey,
        }))
      );
      setIsLoading(false);
      return nonWorkingIntervals;
    } catch (e) {
      console.log(e);
      setIsLoading(false);
    }
  };

  const fetchStylistAppointments = async () => {
    try {
      const appointmentsFromDb = await getStylistAppointments(stylistId);
      setAppointmentsAreLoading(false);
      let currentDate = new Date(Date.now()).toISOString();
      setAppointments(
        appointmentsFromDb.map((app) => ({
          id: app.id,
          start: app.startDate,
          end: app.endDate,
          color:
            app.startDate < currentDate
              ? lightGrey
              : app.statusId === 1
              ? "grey"
              : app.statusId === 2
              ? green
              : red,
        }))
      );
    } catch (e) {
      setAppointmentsAreLoading(false);
    }
  };

  const handleEventClick = async (e) => {
    setClickedAppointmentIsLoading(true);
    try {
      let startDate = e.event.start.toISOString();

      let startDateReq = e.event._instance.range.start.toISOString();

      if (
        unavailableTime.find(
          (elem) => new Date(elem.start).toISOString() === startDate
        )
      ) {
        return;
      }

      let app = await getAppointmentByStartDateAndStylistId(
        stylistId,
        startDateReq
      );

      setClickedAppointment(app);
      setClickedAppointmentIsLoading(false);
    } catch (e) {
      console.log(e);
      setClickedAppointmentIsLoading(false);
    }
  };

  return (
    <div>
      <Stylist></Stylist>
      <FullCalendar
        allDaySlot={false}
        plugins={[timeGridPlugin, interactionPlugin]}
        initialView="timeGridWeek"
        editable
        dateClick={() => {}}
        eventClick={(e) => handleEventClick(e)}
        events={[...appointments, ...unavailableTime]}
        businessHours={schedule.map((s) => ({
          daysOfWeek: [s.weekDayId - 1],
          startTime: s.startTime,
          endTime: s.endTime,
        }))}
      />
    </div>
  );
};

export default StylistPresentationPage;
