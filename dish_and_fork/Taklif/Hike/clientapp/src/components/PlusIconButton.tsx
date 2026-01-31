import { Add } from '@mui/icons-material';
import { FC } from 'react';

import { WhiteIconButton, WhiteIconButtonProps } from './WhiteIconButton';

type PlusIconButtonProps = WhiteIconButtonProps;

const PlusIconButton: FC<PlusIconButtonProps> = (props) => (
  <WhiteIconButton color="primary" {...props}>
    <Add />
  </WhiteIconButton>
);

export { PlusIconButton };
export type { PlusIconButtonProps };
