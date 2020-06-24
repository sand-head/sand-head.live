import { createElement, Children } from '@bikeshaving/crank';
import { Chat } from './Chat';
import { css } from 'emotion';

interface LayoutProps {
  children: Children;
}
export function Layout({ children }: LayoutProps) {
  return (
    <div class={styles}>
      <main>
        {children}
      </main>
      <Chat />
    </div>
  );
}

const styles = css`
  background-color: var(--depth-0);
  display: grid;
  width: 100vw;
  height: 100vh;
  grid-template: auto / auto 350px;
  grid-template-areas: "main chat";

  main {
    grid-area: main;
  }
`;