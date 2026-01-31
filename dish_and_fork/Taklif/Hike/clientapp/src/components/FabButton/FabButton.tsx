import AddIcon from '@mui/icons-material/Add';
import { Fab, FabProps, styled } from '@mui/material';

const StyledFabButton = styled(Fab)(({ theme }) => ({
  bottom: '120px',
  boxShadow: '0px 3px 5px -1px rgb(0 0 0 / 20%), 0px 6px 10px 0px rgb(0 0 0 / 14%), 0px 1px 18px 0px rgb(0 0 0 / 12%)',
  display: 'none',
  position: 'fixed',
  [theme.breakpoints.down('sm')]: {
    display: 'flex',
  },
  right: '20px',
}));

const FabButton = ({ ...props }: FabProps) => {
  return (
    <StyledFabButton aria-label="add" color="primary" {...props}>
      <AddIcon />
    </StyledFabButton>
  );
};

export default FabButton;
