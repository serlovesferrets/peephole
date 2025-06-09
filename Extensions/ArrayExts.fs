module Extensions.ArrayExts

module Array =
    let tap: (array<'t> -> unit) -> array<'t> -> array<'t> =
        fun fn arr ->
            fn arr
            arr
