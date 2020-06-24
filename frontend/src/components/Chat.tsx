import { createElement, Context } from '@bikeshaving/crank';
import { ChatForm } from './ChatForm';
import { css } from 'emotion';
import { ChatLog } from './ChatLog';

export function Chat(this: Context) {
  return (
    <section class={styles}>
      <ChatLog />
      <ChatForm />
    </section>
  );
}

const styles = css`
  background-color: var(--depth-1);
  grid-area: chat;
  display: flex;
  flex-direction: column;
  min-height: 0;

  #messages {
    flex: 1;
    overflow-y: auto;
    min-height: 0;
  }
`;