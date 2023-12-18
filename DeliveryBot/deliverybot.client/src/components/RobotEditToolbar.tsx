import { LocationOn } from "@mui/icons-material";
import { Button } from "@mui/material";
import EditToolbar, { EditToolbarProps } from "./EditToolbar";

export interface RobotEditToolbarProps extends EditToolbarProps {
  setModal: React.Dispatch<React.SetStateAction<boolean>>
}

const RobotEditToolbar: React.FC<RobotEditToolbarProps> = (props) => {
  const { setRows, setRowModesModel, setModal } = props;

  const handleViewLocation = () => {
    setModal(true);
  };

  return (
    <EditToolbar setRows={setRows} setRowModesModel={setRowModesModel}>
      <Button color="primary" startIcon={<LocationOn />} onClick={handleViewLocation}>
        View Location
      </Button>
    </EditToolbar>
  );
};

export default RobotEditToolbar;