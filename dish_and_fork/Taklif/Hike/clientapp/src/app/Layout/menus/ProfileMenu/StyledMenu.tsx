import { Menu, MenuProps, styled } from '@mui/material';

const StyledMenu = styled(
  (props: MenuProps) => (
    <Menu
      anchorOrigin={{
        horizontal: 'right',
        vertical: 'bottom',
      }}
      elevation={0}
      transformOrigin={{
        horizontal: 'right',
        vertical: 'top',
      }}
      {...props}
    />
  ),
  { name: 'StyledMenu' },
)(({ theme }) => ({
  '& .MuiPaper-root': {
    borderColor: 'rgba(0, 0, 0, 0.04)',
    borderStyle: 'solid',
    borderWidth: 1,
  },

  marginTop: theme.spacing(1),
}));

export { StyledMenu };
export type { MenuProps as StyledMenuProps };
