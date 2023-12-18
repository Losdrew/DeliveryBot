import { Add } from "@mui/icons-material";
import { Button } from "@mui/material";
import { GridRowModes, GridRowModesModel, GridRowsProp, GridToolbarContainer } from "@mui/x-data-grid";
import { ReactNode } from "react";

export interface EditToolbarProps {
  children: ReactNode;
  setRows: (newRows: (oldRows: GridRowsProp) => GridRowsProp) => void;
  setRowModesModel: (
    newModel: (oldModel: GridRowModesModel) => GridRowModesModel,
  ) => void;
}

const EditToolbar: React.FC<EditToolbarProps> = (props) => {
  const { setRows, setRowModesModel } = props;

  const handleClick = () => {
    const id = Date.now().toString();
    setRows((oldRows) => [...oldRows, { id, name: '', age: '', isNew: true }]);
    setRowModesModel((oldModel) => ({
      ...oldModel,
      [id]: { mode: GridRowModes.Edit, fieldToFocus: 'name' },
    }));
  };

  return (
    <GridToolbarContainer>
      <Button color="primary" startIcon={<Add />} onClick={handleClick}>
        Add
      </Button>
      { props.children }
    </GridToolbarContainer>
  );
};

export default EditToolbar;