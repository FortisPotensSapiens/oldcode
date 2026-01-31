import { Box, BoxProps, styled } from '@mui/material';

type EllipsisBoxProps = BoxProps & { lineClamp?: number };

const common = {
  overflow: 'hidden',
  textOverflow: 'ellipsis',
};

const EllipsisBox = styled(Box)<EllipsisBoxProps>(({ lineClamp }) => {
  if (!lineClamp) {
    return { ...common, whiteSpace: 'nowrap' };
  }

  return {
    ...common,
    WebkitBoxOrient: 'vertical',
    WebkitLineClamp: lineClamp,
    display: '-webkit-box',
    lineClamp,
  };
});

export { EllipsisBox };
