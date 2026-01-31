import { Box, styled, Typography } from '@mui/material';
import Dialog from '@mui/material/Dialog';

const StyledDialog = styled(Dialog)(({ theme }) => ({
  '.MuiDialog-container': {
    justifyContent: 'flex-end',
    [theme.breakpoints.down('sm')]: {
      alignItems: 'end',
    },
    [theme.breakpoints.up('sm')]: {
      alignItems: 'initial',
    },
  },
}));

const StyledDialogContent = styled(Box)(({ theme }) => ({
  display: 'flex',
  flexDirection: 'column',
  height: '100%',
  padding: theme.spacing(2),
}));

const StyledHeader = styled(Box)(({ theme }) => ({
  alignItems: 'center',
  display: 'flex',
  paddingBottom: theme.spacing(2),
  [theme.breakpoints.down('sm')]: {
    display: 'none',
  },
}));

const StyledTitle = styled(Typography)(({ theme }) => ({
  color: theme.palette.primary.main,
  display: 'inline-block',
}));

const StyledFooter = styled(Box)(({ theme }) => ({
  paddingTop: theme.spacing(2),
}));

const StyledContent = styled(Box)(({ theme }) => ({
  flexGrow: 1,
  overflowY: 'scroll',
}));

export { StyledContent, StyledDialog, StyledDialogContent, StyledFooter, StyledHeader, StyledTitle };
