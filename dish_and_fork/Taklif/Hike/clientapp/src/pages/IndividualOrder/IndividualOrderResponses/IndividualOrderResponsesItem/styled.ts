import { Typography } from '@mui/material';
import { styled } from '@mui/system';

import { ShowMoreTextTextField } from '~/components';

const StyledShowMoreTextTextField = styled(ShowMoreTextTextField)`
  text-overflow: ellipsis;
  overflow: hidden;
`;

const StyledHeader = styled(Typography)`
  display: flex;
  align-items: center;
`;

export { StyledHeader, StyledShowMoreTextTextField };
