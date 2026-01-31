import { FC } from 'react';

import { LoadingSpinner } from '~/components';
import { useCurrency } from '~/hooks';

const Prepare: FC = ({ children }) => {
  const currency = useCurrency();

  if (!currency) {
    return <LoadingSpinner />;
  }

  return <>{children}</>;
};

export { Prepare };
