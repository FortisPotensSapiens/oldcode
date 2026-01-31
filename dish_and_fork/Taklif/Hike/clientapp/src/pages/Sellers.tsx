import { FC } from 'react';

import { usePartners } from '~/hooks';
import { SellersView } from '~/views';

const Sellers: FC = () => {
  const sellers = usePartners();

  return <SellersView {...sellers} />;
};

export default Sellers;
