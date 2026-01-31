import { useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

import { generateRepeatOrderPath } from '~/routing';

type UseRepeat = (orderId: string) => () => void;

const useRepeat: UseRepeat = (orderId) => {
  const navigate = useNavigate();

  return useCallback(() => {
    navigate(generateRepeatOrderPath({ orderId }));
  }, [orderId, navigate]);
};

export { useRepeat };
