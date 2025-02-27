import interactionPlugin from "@fullcalendar/interaction";
import FullCalendar from "@fullcalendar/react";
import timeGridPlugin from "@fullcalendar/timegrid";
import { Box, Modal } from "@mui/material";
import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import useAppointmentService from "../../services/AppointmentService";
import useScheduleService from "../../services/ScheduleService";
import useStylistService from "../../services/StylistService";
import useUnavailableTimeServce from "../../services/UnavailableTimeServce";
import userSession from "../../utils/userSession";

const StylistHomePage = () => {
  const user = userSession.user();
  const [appointments, setAppointments] = useState([]);
  const [clickedAppointment, setClickedAppointment] = useState(undefined);
  const [clickedAppointmentIsLoading, setClickedAppointmentIsLoading] =
    useState(true);
  const [isLoading, setIsLoading] = useState(true);
  const [mergedArray, setMergedArray] = useState(undefined);
  const [schedule, setSchedule] = useState([]);
  const [modalIsOpen, setModalIsOpen] = useState(false);
  const [modalAreYouSureIsOpen, setModalAreYouSureIsOpen] = useState(false);
  const [modalReviewIsOpen, setModalReviewIsOpen] = useState(false);
  const [unavailableTime, setUnavailableTime] = useState([]);
  const [appointmentsareLoading, setAppointmentsAreLoading] = useState(true);
  const { getStylistAppointments, getStylistIdByUserId } = useStylistService();
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
  const style = {
    position: "absolute",
    top: "50%",
    left: "50%",
    transform: "translate(-50%, -50%)",
    width: 400,
    bgcolor: "background.paper",
    border: "2px solid #000",
    boxShadow: 24,
    p: 4,
  };

  const handleModalState = () => {
    setModalIsOpen(!modalIsOpen);
  };

  const handleModalAreYouSure = () => {
    setModalAreYouSureIsOpen(!modalAreYouSureIsOpen);
  };

  const handleModalReviewState = () => {
    setModalReviewIsOpen(!modalReviewIsOpen);
    setModalIsOpen(false);
  };

  const fetchSchedule = async () => {
    try {
      const stylistId = await getStylistIdByUserId(user.id);
      setSchedule(await getScheduleByStylistId(stylistId));
      setIsLoading(false);
    } catch (e) {
      console.log(e);
      setIsLoading(false);
    }
  };

  const fetchUnavailableTime = async () => {
    try {
      const stylistId = await getStylistIdByUserId(user.id);
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
      const stylistId = await getStylistIdByUserId(user.id);
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

      const stylistId = await getStylistIdByUserId(user.id);

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

      let currentDate = new Date(Date.now()).toISOString();

      if (startDateReq < currentDate) {
        setModalReviewIsOpen(!modalReviewIsOpen);
      } else {
        setModalIsOpen(!modalIsOpen);
      }

      console.log(app);
      setClickedAppointment(app);
      setClickedAppointmentIsLoading(false);
    } catch (e) {
      console.log(e);
      setClickedAppointmentIsLoading(false);
    }
  };

  const navigateToLeaveReviewPage = (clientObject) => {
    navigate(`/clientDetailsForStylist/${clientObject.clientId}`);
  };

  const navigateToReviewPage = (clientObject) => {
    navigate(`/clientDetailsReviews/${clientObject.clientId}`);
  };

  const handleAccept = async () => {
    await acceptAppointment(clickedAppointment.id);
  };

  const handleDeny = async () => {
    handleModalAreYouSure();
    if (!modalAreYouSureIsOpen) {
      handleModalState();
    }
    setModalIsOpen(false);
  };

  const handlePageReload = async () => {
    await denyAppointment(clickedAppointment.id);
    handleDeny();
  };

  useEffect(() => {
    fetchStylistAppointments();
  }, [modalIsOpen, modalAreYouSureIsOpen]);

  useEffect(() => {
    fetchSchedule();
    fetchUnavailableTime();
  }, []);

  return (
    <div>
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
      <Modal open={modalIsOpen} onClose={() => handleModalState()}>
        <Box sx={{ ...style }}>
          {!clickedAppointmentIsLoading && (
            <div>
              Client:
              <button
                className="client-name-button"
                onClick={() => navigateToReviewPage(clickedAppointment)}
              >
                {clickedAppointment.clientName}
              </button>
              <div>
                {clickedAppointment.services.map((s) => (
                  <div key={s.serviceId}>{s.service}</div>
                ))}
              </div>
              <button className="accept-button" onClick={handleAccept}>
                Accept
              </button>
              <button className="deny-button" onClick={handleDeny}>
                Deny
              </button>
            </div>
          )}
        </Box>
      </Modal>

      <Modal
        open={modalAreYouSureIsOpen}
        onClose={() => handleModalAreYouSure()}
      >
        <Box sx={{ ...style }}>
          <div>Are you sure you want to deny this appointment?</div>
          <button
            style={{ marginLeft: 90, marginTop: 20 }}
            onClick={() => handlePageReload()}
          >
            Yes
          </button>
          <button
            style={{ marginLeft: 90, marginTop: 20 }}
            onClick={() => handleModalAreYouSure()}
          >
            No
          </button>
        </Box>
      </Modal>

      <Modal open={modalReviewIsOpen} onClose={() => handleModalReviewState()}>
        <Box sx={{ ...style }}>
          {!clickedAppointmentIsLoading && (
            <div>
              <div>{clickedAppointment.clientName}</div>
              <button
                onClick={() => navigateToLeaveReviewPage(clickedAppointment)}
              >
                Leave review
              </button>
            </div>
          )}
        </Box>
      </Modal>
    </div>
  );
};

export default StylistHomePage;
