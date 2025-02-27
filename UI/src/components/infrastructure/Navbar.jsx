import AdbIcon from "@mui/icons-material/Adb";
import MenuIcon from "@mui/icons-material/Menu";
import { Modal } from "@mui/material";
import AppBar from "@mui/material/AppBar";
import Avatar from "@mui/material/Avatar";
import Box from "@mui/material/Box";
import Button from "@mui/material/Button";
import Container from "@mui/material/Container";
import IconButton from "@mui/material/IconButton";
import Menu from "@mui/material/Menu";
import MenuItem from "@mui/material/MenuItem";
import Toolbar from "@mui/material/Toolbar";
import Tooltip from "@mui/material/Tooltip";
import Typography from "@mui/material/Typography";
import * as React from "react";
import { Link, useNavigate } from "react-router-dom";
import logo from "../../assets/images/beautylink-high-resolution-logo-transparent.png";
import { roleTypes } from "../../constants";
import useStylistService from "../../services/StylistService";
import { getFile } from "../../utils";
import userSession from "../../utils/userSession";

//const settings = ["Profile", "Account", "Dashboard"];

function Navbar() {
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

  const [anchorElNav, setAnchorElNav] = React.useState(null);
  const [anchorElUser, setAnchorElUser] = React.useState(null);
  const [modalIsOpen, setModalIsOpen] = React.useState(false);
  const user = userSession.user();
  const navigate = useNavigate();
  const { getStylistIdByUserId } = useStylistService();

  const handleOpenNavMenu = (event) => {
    setAnchorElNav(event.currentTarget);
  };
  const handleOpenUserMenu = (event) => {
    setAnchorElUser(event.currentTarget);
  };

  const handleCloseNavMenu = () => {
    setAnchorElNav(null);
  };

  const handleCloseUserMenu = () => {
    setAnchorElUser(null);
  };

  const handleModalState = (value) => {
    setModalIsOpen(value);
  };

  const handleLogout = () => {
    handleModalState(false);
    setAnchorElUser(null);
    userSession.clearAuthSession();
    navigate("/");
  };

  const handleNavigateReviews = async () => {
    if (user.roleId === roleTypes.clientId) {
      navigate(`/clientReceivedAndLeftReviews/${user.id}`);
    }

    if (user.roleId === roleTypes.stylistId) {
      let stylistId = await getStylistIdByUserId(user.id);
      navigate(`/stylistReceivedAndLeftReviews/${stylistId}`);
    }

    if (user.roleId === roleTypes.managerId) {
      navigate(`/allSalonReviews`);
    }
  };

  const handleNavigateWorkingTime = async () => {
    let stylistId = await getStylistIdByUserId(user.id);
    navigate(`/schedule/${stylistId}`);
  };

  const handleNavigateEditOrAddServices = async () => {
    navigate("/editOrAddServices");
  };

  return (
    <AppBar
      position="static"
      className="app-navbar"
      sx={{
        backgroundColor: "#ffffff",
      }}
    >
      <Container maxWidth="xl">
        <Toolbar disableGutters>
          <AdbIcon sx={{ display: { xs: "none", md: "flex" }, mr: 1 }} />
          <Link to={"/"}>
            <Typography
              variant="h6"
              noWrap
              component="a"
              href="/"
              sx={{
                mr: 2,
                display: { xs: "none", md: "flex" },
                fontFamily: "monospace",
                fontWeight: 700,
                letterSpacing: ".3rem",
                color: "inherit",
                textDecoration: "none",
              }}
            >
              <img src={logo} style={{ width: 150 }} />
            </Typography>
          </Link>
          <Box sx={{ flexGrow: 1, display: { xs: "flex", md: "none" } }}>
            <IconButton
              size="large"
              aria-label="account of current user"
              aria-controls="menu-appbar"
              aria-haspopup="true"
              onClick={handleOpenNavMenu}
              color="inherit"
            >
              <MenuIcon />
            </IconButton>
            <Menu
              id="menu-appbar"
              anchorEl={anchorElNav}
              anchorOrigin={{
                vertical: "bottom",
                horizontal: "left",
              }}
              keepMounted
              transformOrigin={{
                vertical: "top",
                horizontal: "left",
              }}
              open={Boolean(anchorElNav)}
              onClose={handleCloseNavMenu}
              sx={{
                display: { xs: "block", md: "none" },
              }}
            >
              <MenuItem onClick={() => navigate("/salons")}>
                <Typography textAlign="center">Salons</Typography>
              </MenuItem>

              <MenuItem onClick={() => navigate("/salons")}>
                <Typography textAlign="center">Reviews</Typography>
              </MenuItem>

              {user && user.roleId === roleTypes.stylistId && (
                <MenuItem onClick={() => handleNavigateWorkingTime}>
                  <Typography textAlign="center">Working Time</Typography>
                </MenuItem>
              )}

              {user && user.roleId === roleTypes.clientId && (
                <MenuItem onClick={() => navigate("/allAppointments")}>
                  <Typography textAlign="center">Appointments</Typography>
                </MenuItem>
              )}
            </Menu>
          </Box>
          <AdbIcon sx={{ display: { xs: "flex", md: "none" }, mr: 1 }} />
          <Typography
            variant="h5"
            noWrap
            component="a"
            href="#app-bar-with-responsive-menu"
            sx={{
              mr: 2,
              display: { xs: "flex", md: "none" },
              flexGrow: 1,
              fontFamily: "monospace",
              fontWeight: 700,
              letterSpacing: ".3rem",
              color: "inherit",
              textDecoration: "none",
            }}
          >
            <img src={logo} style={{ width: 150 }} />
          </Typography>

          <Box sx={{ flexGrow: 1, display: { xs: "none", md: "flex" } }}>
            <Button
              onClick={() => navigate("/salons")}
              sx={{ my: 2, color: "white", display: "block", margin: 1 }}
            >
              Salons
            </Button>
            {user && (
              <>
                {user.roleId == 3 && (
                  <Button
                    onClick={handleNavigateReviews}
                    sx={{ my: 2, color: "white", display: "block", margin: 1 }}
                  >
                    Reviews
                  </Button>
                )}

                {user.roleId == 3 && (
                  <Button
                    onClick={handleNavigateWorkingTime}
                    sx={{ my: 2, color: "white", display: "block", margin: 1 }}
                  >
                    Working time
                  </Button>
                )}

                {user.roleId == 3 && (
                  <Button
                    onClick={handleNavigateEditOrAddServices}
                    sx={{ my: 2, color: "white", display: "block", margin: 1 }}
                  >
                    Edit Or Add Services
                  </Button>
                )}

                {user.roleId == 2 && (
                  <Button
                    onClick={handleNavigateReviews}
                    sx={{ my: 2, color: "white", display: "block", margin: 1 }}
                  >
                    Reviews
                  </Button>
                )}

                {user.roleId == 2 && (
                  <Button
                    onClick={() => navigate("/allAppointmentsClient")}
                    sx={{ my: 2, color: "white", display: "block", margin: 1 }}
                  >
                    Appointments
                  </Button>
                )}

                {user.roleId == 3 && (
                  <Button
                    onClick={() => navigate("/allAppointmentsStylist")}
                    sx={{ my: 2, color: "white", display: "block", margin: 1 }}
                  >
                    Appointments
                  </Button>
                )}

                {user.roleId == 4 && (
                  <Button
                    onClick={handleNavigateReviews}
                    sx={{ my: 2, color: "white", display: "block", margin: 1 }}
                  >
                    Reviews
                  </Button>
                )}
              </>
            )}
          </Box>
          {user ? (
            <Box sx={{ flexGrow: 0, color: "black" }}>
              {user.firstName} {user.lastName} &nbsp;
              <Tooltip title="Open settings">
                <IconButton onClick={handleOpenUserMenu} sx={{ p: 0 }}>
                  <Avatar alt="nimic" src={getFile(user.profilePicture)} />
                </IconButton>
              </Tooltip>
              <Menu
                sx={{ mt: "45px" }}
                id="menu-appbar"
                anchorEl={anchorElUser}
                anchorOrigin={{
                  vertical: "top",
                  horizontal: "right",
                }}
                keepMounted
                transformOrigin={{
                  vertical: "top",
                  horizontal: "right",
                }}
                open={Boolean(anchorElUser)}
                onClose={handleCloseUserMenu}
              >
                <MenuItem onClick={() => handleModalState(true)}>
                  <Typography textAlign="center">Logout</Typography>
                </MenuItem>
              </Menu>
            </Box>
          ) : (
            <>
              <Link to={"/login"}>
                <MenuItem onClick={handleCloseUserMenu}>
                  <Typography textAlign="center">Login</Typography>
                </MenuItem>
              </Link>
              <Link to={"/register"}>
                <MenuItem onClick={handleCloseUserMenu}>
                  <Typography textAlign="center">Register</Typography>
                </MenuItem>
              </Link>
            </>
          )}
        </Toolbar>
      </Container>
      <Modal open={modalIsOpen} onClose={() => handleModalState(false)}>
        <Box sx={{ ...style }}>
          <h2 id="child-modal-title">Are you sure you want to logout?</h2>
          <button onClick={handleLogout} style={{ marginLeft: 80 }}>
            Logout
          </button>
          <button onClick={() => handleModalState(false)}>Cancel</button>
        </Box>
      </Modal>
    </AppBar>
  );
}
export default Navbar;
