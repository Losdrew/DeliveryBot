import LocationOnIcon from "@mui/icons-material/LocationOn";
import { Button } from "@mui/material";
import EditToolbar, { EditToolbarProps } from "./EditToolbar";
import { useTranslation } from "react-i18next";

export interface RobotEditToolbarProps extends EditToolbarProps {
  setModal: React.Dispatch<React.SetStateAction<boolean>>
}

const RobotEditToolbar: React.FC<RobotEditToolbarProps> = (props) => {
  const { setRows, setRowModesModel, setModal } = props;
  const { t } = useTranslation();

  const handleViewLocation = () => {
    setModal(true);
  };

  return (
    <EditToolbar setRows={setRows} setRowModesModel={setRowModesModel}>
      <Button color="primary" startIcon={<LocationOnIcon />} onClick={handleViewLocation}>
        {t('viewLocation')}
      </Button>
    </EditToolbar>
  );
};

export default RobotEditToolbar;