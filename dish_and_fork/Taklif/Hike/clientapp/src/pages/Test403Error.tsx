import { FC } from 'react';

import { useGetTest403Error } from '~/api';
import { Error } from '~/components';

const Test403Error: FC = () => {
  useGetTest403Error();

  return <Error />;
};

export default Test403Error;
