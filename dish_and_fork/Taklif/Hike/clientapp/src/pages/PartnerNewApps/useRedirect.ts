import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

import { generateIndividualOrderOfferPath } from '~/routing/individualOrder';

const useRedirect = () => {
  const navigate = useNavigate();

  return useCallback(
    (individualOrderId: string, offerId: string) =>
      navigate(generateIndividualOrderOfferPath(individualOrderId, offerId)),
    [navigate],
  );
};

export { useRedirect };
