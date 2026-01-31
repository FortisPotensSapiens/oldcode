import { MouseEventHandler, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';

type UseClick = () => MouseEventHandler<HTMLAnchorElement>;

const useClick: UseClick = () => {
  const navigate = useNavigate();

  return useCallback(
    (event) => {
      event.preventDefault();
      navigate(event.currentTarget.pathname);
    },
    [navigate],
  );
};

export default useClick;
