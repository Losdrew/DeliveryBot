import AddIcon from "@mui/icons-material/Add";
import { Button } from "@mui/material";
import { GridRowModes, GridRowModesModel, GridRowsProp, GridToolbarContainer } from "@mui/x-data-grid";
import { ReactNode } from "react";
import { useTranslation } from "react-i18next";

export interface EditToolbarProps {
  children: ReactNode;
  setRows: (newRows: (oldRows: GridRowsProp) => GridRowsProp) => void;
  setRowModesModel: (
    newModel: (oldModel: GridRowModesModel) => GridRowModesModel,
  ) => void;
}

const EditToolbar: React.FC<EditToolbarProps> = (props) => {
  const { setRows, setRowModesModel } = props;
  const { t } = useTranslation();

  const handleClick = () => {
    const id = Date.now().toString();
    setRows((oldRows) => [...oldRows, { id, isNew: true }]);
    setRowModesModel((oldModel) => ({
      ...oldModel,
      [id]: { mode: GridRowModes.Edit, fieldToFocus: 'name' },
    }));
  };

  return (
    <GridToolbarContainer>
      <Button color="primary" startIcon={<AddIcon />} onClick={handleClick}>
        {t('add')}
      </Button>
      { props.children }
    </GridToolbarContainer>
  );
};

export default EditToolbar;