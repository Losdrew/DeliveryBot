import { Box, IconButton, Menu, MenuItem } from "@mui/material";
import LanguageIcon from '@mui/icons-material/Language';
import { useTranslation } from "react-i18next";
import { useState } from "react";
import { resources } from '../i18n'

const LanguageChangerButton = () => {
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const { i18n } = useTranslation();

  const handleLanguageMenuOpen = (event: React.MouseEvent<HTMLElement>) => {
    setAnchorEl(event.currentTarget);
  };

  const handleLanguageMenuClose = () => {
    setAnchorEl(null);
  };

  const handleChangeLanguage = (language: string) => {
    i18n.changeLanguage(language);
    handleLanguageMenuClose();
  };

  return (
    <Box>
      <IconButton
        size="medium"
        edge="start"
        color="inherit"
        aria-label="language"
        onClick={handleLanguageMenuOpen}
      >
        <LanguageIcon sx={{ fontSize: 30 }} />
      </IconButton>
      <Menu
        id="language-menu"
        anchorEl={anchorEl}
        open={Boolean(anchorEl)}
        onClose={handleLanguageMenuClose}
      >
        {Object.keys(resources).map((locale) => (
          <MenuItem key={locale} onClick={() => handleChangeLanguage(locale)}>
            {locale}
          </MenuItem>
        ))}
      </Menu>
    </Box>
  );
};

export default LanguageChangerButton;