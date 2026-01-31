import { FC } from 'react';

import { useGetPartnerById } from '~/api';
import { useSellerParams } from '~/routing';
import { SellerView } from '~/views';
import SellerFeedbackTabs from '~/views/SellerView/SellerFeedbackTabs';

const Seller: FC = () => {
  const { sellerId = '' } = useSellerParams();
  const seller = useGetPartnerById(sellerId);

  return (
    <>
      <SellerView {...seller} />
      <SellerFeedbackTabs sellerId={sellerId} />
    </>
  );
};

export default Seller;
