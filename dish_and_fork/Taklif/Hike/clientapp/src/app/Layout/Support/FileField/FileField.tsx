import { DeleteOutline } from '@mui/icons-material';
import { Box, IconButton } from '@mui/material';
import { FC } from 'react';

import { BorderBox } from './BorderBox';
import { ChangeHandler } from './types';
import { useAdd } from './useAdd';
import { useDelete } from './useDelete';
import { useDragAndDrop } from './useDragAndDrop';

type FileFieldProps = {
  disabled?: boolean;
  name?: string;
  files: File[];
  onChange: ChangeHandler;
};

const FileField: FC<FileFieldProps> = ({ disabled, files, name, onChange }) => {
  const addHandler = useAdd(files, onChange);
  const deleteHandler = useDelete(files, onChange, disabled);
  const [drag, handlers] = useDragAndDrop(files, onChange, disabled);

  return (
    <>
      {files.length > 0 && (
        <BorderBox alignItems="center" mb={1} width={1}>
          {files.map(({ name }, index) => (
            <Box key={`${name} ${index.toString()}`} display="flex">
              <Box flexGrow={1} overflow="hidden" textOverflow="ellipsis" whiteSpace="nowrap" width={0}>
                {name}
              </Box>

              <Box
                color="error.main"
                component={IconButton}
                data-index={index}
                disabled={disabled}
                mr={-1}
                onClick={deleteHandler}
              >
                <DeleteOutline />
              </Box>
            </Box>
          ))}
        </BorderBox>
      )}

      <Box bgcolor={drag ? 'grey.200' : undefined} component="label" display="block" {...handlers}>
        <BorderBox
          sx={
            disabled
              ? undefined
              : {
                  '&:hover': {
                    cursor: 'pointer',
                  },
                }
          }
          textAlign="center"
        >
          <Box component="span" display={{ sm: 'inline', xs: 'none' }}>
            Перетащите или{' '}
          </Box>

          <Box
            color={disabled ? undefined : 'primary.main'}
            component="span"
            textTransform={{ sm: 'none', xs: 'capitalize' }}
          >
            выберите
          </Box>

          <span> файлы для загрузки</span>
        </BorderBox>

        <Box height={0} position="absolute" visibility="hidden" width={0}>
          <input disabled={disabled} multiple name={name} onChange={addHandler} type="file" />
        </Box>
      </Box>
    </>
  );
};

export { FileField };
export type { FileFieldProps };
