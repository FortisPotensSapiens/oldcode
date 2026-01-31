import { ChangeEvent, Dispatch, SetStateAction, useCallback, useState } from 'react';

type CheckEventHandler = (event: ChangeEvent<HTMLInputElement>, checked: boolean) => void;

type UseChecked = (initial?: boolean) => [boolean, CheckEventHandler, Dispatch<SetStateAction<boolean>>];

const useChecked: UseChecked = (initial = false) => {
  const [state, setState] = useState(initial);
  const eventHandler = useCallback<CheckEventHandler>((event, checked) => setState(checked), []);

  return [state, eventHandler, setState];
};

export { useChecked };
