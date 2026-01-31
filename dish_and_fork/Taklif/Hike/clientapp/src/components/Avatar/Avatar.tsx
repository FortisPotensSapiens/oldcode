import { Box, styled } from '@mui/material';

const StyledAvatar = styled(Box)<{ width: string; height: string }>(({ theme, height, width }) => ({
  backgroundColor: theme.palette.text.blue,
  borderRadius: '100%',
  color: theme.palette.success.contrastText,
  lineHeight: height,
  marginLeft: theme.spacing(2),
  maxHeight: height,
  textAlign: 'center',
  width,
}));

const Avatar = ({
  width = '50px',
  height = '50px',
  userName,
}: {
  width?: string;
  height?: string;
  userName: string;
}) => {
  const text = userName?.slice(0, 2).toUpperCase();

  return (
    <StyledAvatar width={width} height={height}>
      {text}
    </StyledAvatar>
  );
};

export default Avatar;
