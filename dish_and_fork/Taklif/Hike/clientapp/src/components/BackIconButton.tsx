import { ArrowBack } from '@mui/icons-material';
import { FC } from 'react';

import { WhiteIconButton, WhiteIconButtonProps } from './WhiteIconButton';

type BackIconButtonProps = WhiteIconButtonProps;

const BackIconButton: FC<BackIconButtonProps> = (props) => (
  <WhiteIconButton {...props}>
    <ArrowBack />
  </WhiteIconButton>
);

export { BackIconButton };
export type { BackIconButtonProps };
