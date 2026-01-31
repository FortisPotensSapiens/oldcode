import { styled } from '@mui/material';

const StyledItem = styled('li', { name: 'StyledItem' })(({ theme }) => ({ paddingLeft: theme.spacing(1) }));

export default StyledItem;
