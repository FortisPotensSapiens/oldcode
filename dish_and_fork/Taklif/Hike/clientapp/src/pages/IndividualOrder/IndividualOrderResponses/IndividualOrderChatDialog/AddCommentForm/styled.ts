import SendIcon from '@mui/icons-material/Send';
import { Box, InputBase, styled } from '@mui/material';

const StyledContainer = styled(Box)(({ theme }) => ({
  display: 'flex',
}));

const StyledInput = styled(InputBase)(({ theme }) => ({
  border: `1px solid ${theme.palette.common.grey}`,
  borderRadius: '10px',
  padding: theme.spacing(1),
}));

const StyledSendIcon = styled(SendIcon)`
  cursor: pointer;
`;

export { StyledContainer, StyledInput, StyledSendIcon };
