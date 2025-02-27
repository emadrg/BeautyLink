import {
  Box,
  Container,
  Grid,
  IconButton,
  Stack,
  Typography,
} from "@mui/material";
import { FaInstagram, FaTwitter, FaYoutube } from "react-icons/fa";

const SocialButton = ({ children, label, href }) => {
  return (
    <IconButton
      href={href}
      aria-label={label}
      sx={{
        backgroundColor: "rgba(0, 0, 0, 0.1)",
        width: 40,
        height: 40,
        display: "inline-flex",
        alignItems: "center",
        justifyContent: "center",
        "&:hover": {
          backgroundColor: "rgba(0, 0, 0, 0.2)",
        },
      }}
    >
      {children}
    </IconButton>
  );
};

export default function Footer() {
  return (
    <Box
      sx={{
        position: "relative",
        bottom: 0,
        width: "100%",
        mt: "auto",
        bgcolor: (theme) =>
          theme.palette.mode === "light"
            ? "lightPallette.background.main"
            : "darkPallette.background.main",
        color: (theme) =>
          theme.palette.mode === "light" ? "gray.700" : "white",
      }}
    >
      <Box sx={{ backgroundColor: "rgba(0,0,0,0.1)" }}>
        <Container
          component={Stack}
          maxWidth="xl"
          py={4}
          direction={{ xs: "column", md: "row" }}
          spacing={4}
          justifyContent={{ xs: "center", md: "space-between" }}
          alignItems={{ xs: "center", md: "center" }}
        >
          <Grid container direction="column" alignItems="flex-start">
            <Typography>Contact: 0712345678</Typography>
            <Typography>Email: personal_stuff@academie.com</Typography>
          </Grid>
          <Typography>
            © 2022 Material-UI Templates. All rights reserved
          </Typography>
          <Stack direction="row" spacing={2}>
            <SocialButton label="Twitter" href="#">
              <FaTwitter />
            </SocialButton>
            <SocialButton label="YouTube" href="#">
              <FaYoutube />
            </SocialButton>
            <SocialButton label="Instagram" href="#">
              <FaInstagram />
            </SocialButton>
          </Stack>
        </Container>
      </Box>
    </Box>
  );
}
