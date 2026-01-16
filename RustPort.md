# Rust Port Library Alternatives

This document maps the current .NET dependencies to possible Rust equivalents. It is not a one-to-one mapping in all cases; several .NET libraries are framework-specific and require choosing a Rust UI stack.

## Core

### Riok.Mapperly
Rust alternatives:
- https://crates.io/crates/automapper
- https://crates.io/crates/model-mapper
- https://crates.io/crates/mapper
- https://crates.io/crates/frunk

Notes:
- Rust commonly uses explicit conversion functions or `From`/`Into` implementations; `frunk` can help with structural transformations between types.

### StronglyTypedId
Rust alternatives:
- https://crates.io/crates/typed_id
- https://crates.io/crates/strong_id
- https://crates.io/crates/newtype_derive
- https://crates.io/crates/derive_more

Notes:
- Newtype wrappers are idiomatic in Rust; the crates above reduce boilerplate.

### Microsoft.Extensions.Logging.Debug
Rust alternatives:
- https://docs.rs/log
- https://docs.rs/env_logger
- https://docs.rs/tracing
- https://docs.rs/tracing-subscriber

Notes:
- `log` + `env_logger` is the classic logging combo; `tracing` is the structured, async-friendly option.

### PolySharp
Rust alternatives:
- No direct equivalent (C# polyfill package).

Notes:
- Use Rust language features, `cfg` flags, and backports where needed.

### MessagePipe
Rust alternatives:
- https://docs.rs/tokio/latest/tokio/sync/broadcast/index.html
- https://docs.rs/async-broadcast
- https://docs.rs/flume
- https://docs.rs/crossbeam-channel

Notes:
- Choose between async broadcast channels or multi-producer/multi-consumer channels based on usage.

## UI / MAUI Stack

### Microsoft.Maui.Controls.Core
Rust alternatives:
- https://docs.slint.dev/
- https://github.com/iced-rs/iced
- https://github.com/emilk/egui
- https://github.com/DioxusLabs/dioxus
- https://github.com/gtk-rs/gtk4-rs
- https://tauri.app/

### Microsoft.Maui.Controls
Rust alternatives:
- https://docs.slint.dev/
- https://github.com/iced-rs/iced
- https://github.com/emilk/egui
- https://github.com/DioxusLabs/dioxus
- https://github.com/gtk-rs/gtk4-rs
- https://tauri.app/

### Microsoft.Maui.Graphics
Rust alternatives:
- https://github.com/rust-skia/rust-skia
- https://github.com/gfx-rs/wgpu
- https://github.com/linebender/tiny-skia

Notes:
- `skia-safe` (rust-skia) is the closest Skia binding. `wgpu` is a modern GPU API for custom rendering.

### Microsoft.Maui.Essentials
Rust alternatives:
- https://tauri.app/
- https://github.com/rust-windowing/winit
- Platform-specific crates (Windows, macOS, iOS, Android) as needed.

Notes:
- Essentials-style APIs vary by target; UI framework choice will influence available platform integrations.

### Microsoft.Maui.Controls.Foldable
Rust alternatives:
- Platform-specific APIs (Android foldables/dual-screen features).

Notes:
- No direct Rust cross-platform equivalent; likely requires per-platform integration.

### Plugin.Maui.Audio
Rust alternatives:
- https://docs.rs/rodio
- https://docs.rs/cpal
- https://docs.rs/kira
- https://docs.rs/symphonia

Notes:
- `cpal` for low-level audio I/O, `rodio`/`kira` for playback, `symphonia` for decoding.

### ThomasLevesque.WeakEvent
Rust alternatives:
- https://doc.rust-lang.org/std/rc/struct.Weak.html
- https://crates.io/crates/weak-table

Notes:
- Use `Weak` refs to avoid reference cycles; `weak-table` can help manage weak registries.

### Sharpnado.Maui.Shadows
Rust alternatives:
- Use the chosen UI toolkit's styling or custom drawing.
- https://docs.slint.dev/
- https://github.com/emilk/egui
- https://github.com/iced-rs/iced
- https://github.com/gtk-rs/gtk4-rs

Notes:
- Toolkits typically support shadows via styling or custom paint; verify capabilities per toolkit.

### CommunityToolkit.Maui
Rust alternatives:
- https://docs.slint.dev/
- https://github.com/emilk/egui
- https://github.com/iced-rs/iced
- https://github.com/gtk-rs/gtk4-rs
- https://github.com/DioxusLabs/dioxus

## Reactive / MVVM

### ReactiveUI
Rust alternatives:
- https://docs.rs/rxrust
- https://docs.rs/futures
- https://docs.rs/tokio-stream
- https://docs.rs/futures-signals

Notes:
- Prefer `Stream`-based reactive flows; `rxrust` provides an Rx-style API.

### ReactiveUI.Maui
Rust alternatives:
- https://docs.rs/rxrust
- https://docs.rs/futures
- https://docs.rs/tokio-stream
- https://docs.rs/futures-signals
- https://docs.slint.dev/
- https://github.com/emilk/egui
- https://github.com/iced-rs/iced
- https://github.com/gtk-rs/gtk4-rs
- https://github.com/DioxusLabs/dioxus

### ReactiveUI.SourceGenerators
Rust alternatives:
- https://crates.io/crates/derive_more

Notes:
- Use procedural macros (custom derive) for boilerplate generation.

### ReactiveUI.SourceGenerators.Analyzers.CodeFixes
Rust alternatives:
- https://github.com/rust-lang/rust-clippy

Notes:
- Clippy provides linting and suggestions analogous to analyzer/code-fix tooling.

### DynamicData
Rust alternatives:
- https://docs.rs/futures
- https://docs.rs/tokio-stream
- https://docs.rs/rxrust

Notes:
- Combine streams with collection diffing logic in domain code or helper crates.

### ReactiveMarbles.ObservableEvents.SourceGenerator
Rust alternatives:
- https://docs.rs/tokio/latest/tokio/sync/broadcast/index.html
- https://docs.rs/async-broadcast
- https://docs.rs/tokio-stream

Notes:
- Emit events through channels and adapt them into `Stream`s.

## Skia / SVG / Drawing

### SkiaSharp
Rust alternatives:
- https://github.com/rust-skia/rust-skia

### SkiaSharp.Views.Maui.Controls
Rust alternatives:
- https://github.com/rust-skia/rust-skia
- https://docs.slint.dev/ (Slint supports a Skia renderer)

### nor0x.Maui.ColorPicker
Rust alternatives:
- https://github.com/emilk/egui (includes a color picker widget)
- https://docs.slint.dev/
- https://github.com/iced-rs/iced
- https://github.com/gtk-rs/gtk4-rs
- https://github.com/DioxusLabs/dioxus

### SkiaSharp.Extended
Rust alternatives:
- https://github.com/rust-skia/rust-skia
- https://github.com/linebender/tiny-skia
- https://github.com/RazrFalcon/resvg

### Svg.Skia
Rust alternatives:
- https://github.com/RazrFalcon/resvg
- https://github.com/RazrFalcon/usvg
- https://github.com/linebender/tiny-skia

## Testing

### Microsoft.NET.Test.Sdk
Rust alternatives:
- https://doc.rust-lang.org/book/ch11-01-writing-tests.html

Notes:
- `cargo test` uses Rust's built-in test harness.

### xunit
Rust alternatives:
- https://doc.rust-lang.org/book/ch11-01-writing-tests.html
- https://docs.rs/rstest

### xunit.runner.visualstudio
Rust alternatives:
- https://doc.rust-lang.org/book/ch11-01-writing-tests.html

Notes:
- IDEs generally integrate with `cargo test`.

### coverlet.collector
Rust alternatives:
- https://github.com/taiki-e/cargo-llvm-cov
- https://github.com/xd009642/tarpaulin

### Shouldly
Rust alternatives:
- https://docs.rs/pretty_assertions
- https://docs.rs/assert2
- https://insta.rs/

### Shouldly.FromAssert
Rust alternatives:
- https://docs.rs/pretty_assertions
- https://docs.rs/assert2
- https://insta.rs/
