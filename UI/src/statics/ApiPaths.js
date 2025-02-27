export default {
  Authenticate: "/auth",
  GetUserById: (id) => `/user/${id}`,
  GetUsers: "/users",
  RegisterUser: "/user/register",
  RegisterStylist: "/stylists/register",
  RegisterManager: "/managers/register",
  UpdateUser: "/user/update",
  DeactivateUser: (userId) => `/user/deactivate/${userId}`,
  DeleteUser: (userId) => `/user/delete/${userId}`,
  GetRoles: "/roles",
  GetCities: "/cities",
  GetCitiesByCountyId: (countyId) => `/cities/${countyId}`,
  GetCounties: "/counties",
  GetSalons: (serviceId, skip, take) =>
    `/salons${
      skip != null && take != null ? `?skip=${skip}&take=${take}&` : `?`
    }${serviceId ? `serviceId=${serviceId}` : ""}`,
  GetSalonsByCityIdAndServiceId: (cityId, serviceId) =>
    `/salons/getByCity?cityId=${cityId}&${
      serviceId ? `serviceId=${serviceId}` : ""
    }`,
  GetSalonSuggestions: (cityId) => `/salons/suggestions/${cityId}`,
  CreateSalon: "salons/create",
  GetSalonWithDetails: (id) => `/salons/${id}`,
  GetServices: "/services",
  GetStylistById: (stylistId) => `/stylists/${stylistId}`,
  GetServiceById: (serviceId) => `/services/${serviceId}`,
  GetStylistWithServices: (stylistId) =>
    `/stylists/getStylistWithServices/${stylistId}`,
  GetScheduleByStylisId: (stylistId) => `/schedule/${stylistId}`,
  GetUnavailableTimeByStylistId: (stylistId) => `/unavailableTime/${stylistId}`,
  CreateAppointment: "/appointment/create",
  UpdateClientStylistReview: "/reviewsClientStylist/update",
  GetClientSalonReviewsBySalonId: (salonId, skip, take) =>
    `/reviewsClientSalon/${salonId}${
      skip != null && take != null ? `?skip=${skip}&take=${take}` : ``
    }`,
  GetStylistAppointmentsByStylistId: (stylistId) => `/appointment/${stylistId}`,
  GetClientDetailsByClientId: (clientId) => `/client/${clientId}`,
  CreateStylistClientReview: "/reviewsStylistClient/create",
  CreateClientStylistReview: "/reviewsClientStylist/create",
  GetProfilePictureName: (userId) => `/user/profilePicture/${userId}`,
  GetSalonPictures: (salonId, skip, take) =>
    `/salons/pictures/${salonId}${
      skip != null && take != null ? `?skip=${skip}&take=${take}` : ``
    }`,
  GetLastVisitedSalonByClientId: (clientId) =>
    `/salons/lastVisited/${clientId}`,
  GetStylistAllAppointmentsByStylistId: (stylistId) =>
    `/stylists/getAppointments/${stylistId}`,
  GetStylistIdByUserId: (userId) => `/stylists/getStylistIdByUserId/${userId}`,
  GetAppointmentByStartDateAndStylistId: (stylistId, startDate) =>
    `/appointment/identify/${stylistId}/${startDate}`,
  AcceptAppointment: (appointmentId) => `/appointment/accept/${appointmentId}`,
  DenyAppointment: (appointmentId) => `/appointment/deny/${appointmentId}`,

  UpdateStylistClientReview: "/reviewsStylistClient/update",

  GetClientStylistReviewsByStylistId: (stylistId) =>
    `/reviewsClientStylist/${stylistId}`,

  GetClientReviewsLeftByStylist: (stylistId) =>
    `/reviewsStylistClient/leftByStylist/${stylistId}`,

  GetStylistClientReviewsByClientId: (clientId) =>
    `/reviewsStylistClient/${clientId}`,

  GetStylistReviewsLeftByClient: (clientId) =>
    `/reviewsClientStylist/leftByClient/${clientId}`,

  GetWeekdays: "/schedule/weekdays",
  CreateOrUpdateSchedule: (stylistId) =>
    `/schedule/createOrUpdate/${stylistId}`,
  DeleteSchedule: (stylistId, weekdayId) =>
    `/schedule/delete/${stylistId}/${weekdayId}`,

  GetStylistsByManagerId: (managerId) =>
    `/managers/getAllStylistsFromSalon/${managerId}`,

  GetManagerIdByUserId: (userId) => `/managers/getManagerId/${userId}`,
  GetStylistsReviewsByManagerId: (managerId) =>
    `/managers/stylistsReviews/${managerId}`,

  CreateClientSalonReview: "/reviewsClientSalon/create",
  UpdateClientSalonReview: "/reviewsClientSalon/update",
  GetSalonIdByManagerId: (managerId) => `/salons/getIdByManagerId/${managerId}`,

  HasClientVisitedSalon: (salonId) => `/salons/hasClientVisited/${salonId}`,
  HasClientVisitedStylist: (stylistId) =>
    `/stylists/hasClientVisited/${stylistId}`,

  createUnavailableTime: (stylistId) => `/unavailableTime/create/${stylistId}`,
  GetClientAllAppointments: "/appointment/clientAll",
  GetStylistAllAppointments: "/appointment/stylistAll",
  GetStylistByFiltering: (startDate, endDate, serviceId, cityId) =>
    `/stylists/filerStylists/${startDate}/${endDate}/${serviceId}/${cityId}`,
  GetServicesForCurrentStylist: "/serviceStylists",
  UpdateServiceForCurrentStylist: "/serviceStylists/update",
  CreateServiceForCurrentStylist: "/serviceStylists/create",
  DeleteServiceForCurrentStylist: (id) => `/serviceStylists/delete/${id}`,
  GetAllUsers: (roleId, skip, take) =>
    `/user/getAll/${skip}/${take}${roleId != null ? `?roleId=${roleId}` : ``}`,
};
